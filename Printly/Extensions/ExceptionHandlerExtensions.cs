using Microsoft.AspNetCore.Builder;
using Printly.Services;
using System.Threading;

namespace Printly.Extensions
{
    public static class ExceptionHandlerExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var exceptionHandler = (IExceptionHandlerService)appBuilder.ApplicationServices.GetService(typeof(IExceptionHandlerService));
                    if (exceptionHandler != null)
                    {
                        await exceptionHandler.HandleAsync(
                            context,
                            CancellationToken.None);
                    }
                });
            });
        }
    }
}
