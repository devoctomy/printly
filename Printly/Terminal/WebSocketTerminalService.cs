using Printly.Services;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Terminal
{
    public class WebSocketTerminalService : IWebSocketTerminalService
    {
        private readonly WebSocket _webSocket;
        private readonly WebSocketTerminalServiceConfiguration _webSocketTerminalServiceConfiguration;

        public WebSocketTerminalService(
            WebSocket webSocket,
            WebSocketTerminalServiceConfiguration webSocketTerminalServiceConfiguration)
        {
            _webSocket = webSocket;
            _webSocketTerminalServiceConfiguration = webSocketTerminalServiceConfiguration;
        }

        public async Task RunCommsLoopAsync(
            ISerialPortCommunicationService serialPortCommunicationService,
            CancellationToken cancellationToken)
        {
            try
            {
                var buffer = new byte[_webSocketTerminalServiceConfiguration.ReceiveBufferSize];
                WebSocketReceiveResult result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                while (!result.CloseStatus.HasValue)
                {
                    serialPortCommunicationService.Write(
                        buffer,
                        0,
                        result.Count);
                    result = await _webSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer),
                        cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                //Gracefully shut down
            }

            if (_webSocket.State == WebSocketState.Open ||
                _webSocket.State == WebSocketState.CloseReceived)
            {
                await _webSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Connection forcefully closed from server side.",
                    CancellationToken.None);
            }
        }
    }
}
