using Microsoft.Extensions.Logging;
using Printly.Services;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Terminal
{
    public class WebSocketTerminalService : IWebSocketTerminalService
    {
        private readonly WebSocketTerminalServiceConfiguration _webSocketTerminalServiceConfiguration;
        private readonly ILogger<WebSocketTerminalService> _logger;

        public WebSocket WebSocket { get; set; }

        public WebSocketTerminalService(
            WebSocketTerminalServiceConfiguration webSocketTerminalServiceConfiguration,
            ILogger<WebSocketTerminalService> logger)
        {
            _webSocketTerminalServiceConfiguration = webSocketTerminalServiceConfiguration;
            _logger = logger;
        }

        public async Task RunCommsLoopAsync(
            ISerialPortCommunicationService serialPortCommunicationService,
            CancellationToken cancellationToken)
        {
            try
            {
                var buffer = new byte[_webSocketTerminalServiceConfiguration.ReceiveBufferSize];
                WebSocketReceiveResult result = await WebSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    cancellationToken);
                var rawDataString = serialPortCommunicationService.SerialPort.Encoding.GetString(
                    buffer,
                    0,
                    result.Count);
                while (!result.CloseStatus.HasValue)
                {
                    var dataString = rawDataString;
                    if (!dataString.EndsWith('\n'))
                    {
                        dataString += '\n';
                    }
                    _logger.LogDebug($"TX {serialPortCommunicationService.SerialPort.PortName} >> {dataString.TrimEnd()}");
                    buffer = serialPortCommunicationService.SerialPort.Encoding.GetBytes(dataString);
                    serialPortCommunicationService.Write(
                        buffer,
                        0,
                        buffer.Length);

                    await Task.Yield();

                    buffer = new byte[_webSocketTerminalServiceConfiguration.ReceiveBufferSize];
                    result = await WebSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer),
                        cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                //Gracefully shut down
            }

            if (WebSocket.State == WebSocketState.Open ||
                WebSocket.State == WebSocketState.CloseReceived)
            {
                await WebSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Connection forcefully closed from server side.",
                    CancellationToken.None);
            }
        }
    }
}
