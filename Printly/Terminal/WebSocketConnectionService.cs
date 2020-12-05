using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Printly.Terminal
{
    public class WebSocketConnectionService : IWebSocketConnectionService
    {
        private readonly WebSocketTerminalServiceConfiguration _webSocketTerminalServiceConfiguration;
        private readonly ILogger<WebSocketTerminalService> _logger;

        public WebSocketConnectionService(
            WebSocketTerminalServiceConfiguration webSocketTerminalServiceConfiguration,
            ILogger<WebSocketTerminalService> logger)
        {
            _webSocketTerminalServiceConfiguration = webSocketTerminalServiceConfiguration;
            _logger = logger;
        }

        public async Task<IWebSocketTerminalService> Create(HttpContext httpContext)
        {
            if (httpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();
                var webSocketTerminalService = new WebSocketTerminalService(
                    _webSocketTerminalServiceConfiguration,
                    _logger)
                {
                    WebSocket = webSocket
                };
                return webSocketTerminalService;
            }
            else
            {
                return null;
            }
        }
    }
}
