using Microsoft.AspNetCore.Http;
using Printly.Services;
using Printly.Terminal;
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
        private readonly IWebSocketConnectionService _webSocketConnectionService;

        public TerminalMiddleware(
            RequestDelegate next,
            ISerialPortConnectionManager serialPortConnectionManager,
            IWebSocketConnectionService webSocketConnectionService)
        {
            _next = next;
            _serialPortConnectionManager = serialPortConnectionManager;
            _webSocketConnectionService = webSocketConnectionService;
        }

        public async Task Invoke(
            HttpContext httpContext,
            ISerialPortMonitorService serialPortMonitorService)
        {
            if (httpContext.Request.Path.ToString().StartsWith("/terminal"))
            {
                var connection = await _webSocketConnectionService.Create(httpContext);
                if(connection != null)
                {
                    var serialPortCommunicationService = _serialPortConnectionManager.GetOrOpen(httpContext.Request);
                    var cancellationTokenSource = new CancellationTokenSource();
                    serialPortCommunicationService.State = connection.WebSocket;
                    serialPortCommunicationService.DataReceived += SerialPortCommunicationService_DataReceived;

                    serialPortMonitorService.PortsDisconnected += (object sender, PortsDisconnectedEventArgs e) =>
                    {
                        if (e.SerialPorts.Any(x => x.ToLower() == serialPortCommunicationService.PortName.ToLower()))
                        {
                            serialPortCommunicationService.Close();
                            cancellationTokenSource.Cancel();
                        }
                    };
                    serialPortMonitorService.Start();

                    await connection.RunCommsLoopAsync(
                        serialPortCommunicationService,
                        cancellationTokenSource.Token);

                    await serialPortMonitorService.Stop();
                    _serialPortConnectionManager.Close(serialPortCommunicationService.PortName);
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

        private async void SerialPortCommunicationService_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            switch (e.EventType)
            {
                case SerialData.Chars:
                    {
                        var communicationService = ((ISerialPortCommunicationService)sender);
                        var webSocket = (WebSocket)communicationService.State;
                        var serialPort = (ISerialPort)communicationService.SerialPort;
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
        }
    }
}