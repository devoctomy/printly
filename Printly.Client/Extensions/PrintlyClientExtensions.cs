using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace Printly.Client.Extensions
{
    public static class PrintlyClientExtensions
    {
        public static IServiceCollection AddPrintlyClient(
            this IServiceCollection services,
            PrintlyClientConfiguration configuration)
        {
            services.AddHttpClient<IHttpAdapter<SystemClient>,HttpAdapter<SystemClient>> (client =>
            {
                client.BaseAddress = new Uri(configuration.BaseUrl);
            })
                .AddPolicyHandler(GetRetryPolicy(
                    configuration.RetryCount,
                    configuration.SleepDuration))
                .SetHandlerLifetime(configuration.HttpMessageHandlerLifetime);

            services.AddHttpClient<IHttpAdapter<PrintersClient>, HttpAdapter<PrintersClient>>(client =>
            {
                client.BaseAddress = new Uri(configuration.BaseUrl);
            })
                .AddPolicyHandler(GetRetryPolicy(
                    configuration.RetryCount,
                    configuration.SleepDuration))
                .SetHandlerLifetime(configuration.HttpMessageHandlerLifetime);

            services.AddTransient<ISystemClient, SystemClient>();
            services.AddTransient<IPrintersClient, PrintersClient>();
            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(
            int retryCount,
            TimeSpan sleepDuration)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(retryCount, retryAttempt => sleepDuration);
        }
    }
}
