using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Printly.Terminal
{
    public class WebSocketConnectionService : IWebSocketConnectionService
    {
        private readonly WebSocketTerminalServiceConfiguration _webSocketTerminalServiceConfiguration;

        public WebSocketConnectionService(WebSocketTerminalServiceConfiguration webSocketTerminalServiceConfiguration)
        {
            _webSocketTerminalServiceConfiguration = webSocketTerminalServiceConfiguration;
        }

        public async Task<IWebSocketTerminalService> Create(HttpContext httpContext)
        {
            if (httpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();
                var webSocketTerminalService = new WebSocketTerminalService(_webSocketTerminalServiceConfiguration)
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
