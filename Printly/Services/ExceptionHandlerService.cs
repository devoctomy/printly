using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Printly.Dto.Response;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Services
{
    public class ExceptionHandlerService : IExceptionHandlerService
    {
        public async Task HandleAsync(
            HttpContext httpContext,
            CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";
            var exceptionHandlerFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
            if (exceptionHandlerFeature != null)
            {
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new Response
                {
                    Success = false,
                    Error = new Error
                    {
                        HttpStatusCode = (HttpStatusCode)httpContext.Response.StatusCode,
                        Message = exceptionHandlerFeature.Error.Message
                    }
                }), cancellationToken);
            }
        }
    }
}
