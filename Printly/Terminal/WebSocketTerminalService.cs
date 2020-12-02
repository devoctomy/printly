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

        public WebSocket WebSocket { get; set; }

        public WebSocketTerminalService(WebSocketTerminalServiceConfiguration webSocketTerminalServiceConfiguration)
        {
            _webSocketTerminalServiceConfiguration = webSocketTerminalServiceConfiguration;
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
                while (!result.CloseStatus.HasValue)
                {
                    serialPortCommunicationService.Write(
                        buffer,
                        0,
                        result.Count);
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
