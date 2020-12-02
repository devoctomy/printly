using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Services
{
    public interface IExceptionHandlerService
    {
        Task HandleAsync(
            HttpContext httpContext,
            CancellationToken cancellationToken);
    }
}
