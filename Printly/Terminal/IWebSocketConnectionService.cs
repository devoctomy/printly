using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Printly.Terminal
{
    public interface IWebSocketConnectionService
    {
        Task<IWebSocketTerminalService> Create(HttpContext httpContext);
    }
}
