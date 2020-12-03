using Microsoft.AspNetCore.Builder;
using Printly.Services;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Printly.Extensions
{
    public static class ExceptionHandlerExtensions
    {
        [ExcludeFromCodeCoverage] // Not sure if this is possible to unit test so excluding.
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
