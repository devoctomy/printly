using Microsoft.AspNetCore.Http;
using Printly.Services;
using System;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Middleware
{
    public class TerminalMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ISerialPortConnectionManager _serialPortConnectionManager;

        public TerminalMiddleware(
            RequestDelegate next,
            ISerialPortConnectionManager serialPortConnectionManager)
        {
            _next = next;
            _serialPortConnectionManager = serialPortConnectionManager;
        }

        public async Task Invoke(
            HttpContext httpContext,
            ISerialPortMonitorService serialPortMonitorService)
        {
            if (httpContext.Request.Path.StartsWithSegments(new PathString("/terminal")))
            {
                if (httpContext.WebSockets.IsWebSocketRequest)
                {
                    var pathParts = httpContext.Request.Path.ToString().Substring(1).Split("/");
                    var portName = pathParts[1];

                    var connection = _serialPortConnectionManager.GetOrOpen(
                        portName,
                        int.Parse(GetQueryValueOrDefault(httpContext.Request.Query, "baudrate", "9600")),
                        Enum.Parse<Parity>(GetQueryValueOrDefault(httpContext.Request.Query, "parity", Parity.None.ToString()), true),
                        int.Parse(GetQueryValueOrDefault(httpContext.Request.Query, "databits", "8")),
                        Enum.Parse<StopBits>(GetQueryValueOrDefault(httpContext.Request.Query, "stopbits", StopBits.One.ToString()), true),
                        Enum.Parse<Handshake>(GetQueryValueOrDefault(httpContext.Request.Query, "handshake", Handshake.None.ToString()), true),
                        new TimeSpan(1, 0, 0),
                        new TimeSpan(1, 0, 0));
                    using (WebSocket webSocket = await httpContext.WebSockets.AcceptWebSocketAsync())
                    {
                        await CommsLoop(
                            httpContext,
                            webSocket,
                            connection,
                            serialPortMonitorService);
                    }
                }
                else
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                await _next.Invoke(httpContext);
            }
        }

        private string GetQueryValueOrDefault(
            IQueryCollection queryCollection,
            string name,
            string defaultValue)
        {
            if(queryCollection.TryGetValue(name, out var values))
            {
                return values[0];
            }
            else
            {
                return defaultValue;
            }
        }

        private async Task CommsLoop(
            HttpContext context,
            WebSocket webSocket,
            ISerialPortCommunicationService serialPortCommunicationService,
            ISerialPortMonitorService serialPortMonitorService)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var buffer = new byte[1024 * 4];

            serialPortCommunicationService.DataReceived += async (object sender, SerialDataReceivedEventArgs e) =>
            {
                switch(e.EventType)
                {
                    case SerialData.Chars:
                        {
                            var serialPort = (SerialPort)sender;
                            var data = serialPort.ReadExisting();
                            var dataBytes = serialPort.Encoding.GetBytes(data);
                            await webSocket.SendAsync(
                                new ArraySegment<byte>(dataBytes, 0, dataBytes.Length),
                                WebSocketMessageType.Text,
                                true,
                                CancellationToken.None);
                            break;
                        }
                    default:
                        {
                            // Do nothing
                            break;
                        }
                }
            };

            serialPortMonitorService.PortsDisconnected += async (object sender, PortsDisconnectedEventArgs e) =>
            {
                if (e.SerialPorts.Any(x => x.ToLower() == serialPortCommunicationService.PortName.ToLower()))
                {
                    var dataBytes = Encoding.ASCII.GetBytes($"Serial port '{serialPortCommunicationService.PortName}' no longer available.");
                    await webSocket.SendAsync(
                        new ArraySegment<byte>(dataBytes, 0, dataBytes.Length),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None);
                    serialPortCommunicationService.Close();
                    cancellationTokenSource.Cancel();
                }
            };
            serialPortMonitorService.Start();

            WebSocketReceiveResult result = null;
            try
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationTokenSource.Token);
                while (!result.CloseStatus.HasValue)
                {
                    serialPortCommunicationService.Write(
                        buffer,
                        0,
                        result.Count);
                    result = await webSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer),
                        cancellationTokenSource.Token);
                }
            }
            catch(OperationCanceledException ex)
            {
                //Gracefully shut down
            }

            if(webSocket.State == WebSocketState.Open ||
                webSocket.State == WebSocketState.CloseReceived)
            {
                await webSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Connection forcefully closed from server side.",
                    CancellationToken.None);
            }
            serialPortCommunicationService.DataReceived -= dataReceivedHandler;
            serialPortMonitorService.Stop();
            serialPortMonitorService.PortsDisconnected -= portsDisconnectedHandler;
            _serialPortConnectionManager.Close(serialPortCommunicationService.PortName);
        }
    }
}
