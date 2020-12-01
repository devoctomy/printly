using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Printly.Dto.Response;
using System.Net;

namespace Printly.Extensions
{
    public static class ExceptionHandlerExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(exceptionHandlerFeature != null)
                    {
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new Response()
                        {
                            Success = false,
                            Error = new Error
                            {
                                HttpStatusCode = (HttpStatusCode)context.Response.StatusCode,
                                Message = exceptionHandlerFeature.Error.Message
                            }
                        }));
                    }
                });
            });
        }
    }
}
