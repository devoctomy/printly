using Microsoft.Extensions.DependencyInjection;
using Printly.Services;
using Printly.System;

namespace Printly.Extensions
{
    public static class PrintlyServicesExtensions
    {
        public static IServiceCollection AddPrintlyServices(
            this IServiceCollection services,
            AppSettings appSettings)
        {
            services.AddSingleton<SerialPortMonitorServiceConfiguration>(config =>
            {
                return new SerialPortMonitorServiceConfiguration
                {
                    PollPauseMilliseconds = appSettings.SerialPortPollPauseMilliseconds
                };
            });
            services.AddSingleton<IExceptionHandlerService, ExceptionHandlerService>();
            services.AddSingleton<IDateTimeService, DateTimeService>();
            services.AddSingleton<ISystemStateService, SystemStateService>();
            services.AddSingleton<ISerialPortFactory, NativeSerialPortFactory>();
            services.AddSingleton<ISerialPortDiscoveryService, NativeSerialPortDiscoveryService>();
            services.AddSingleton<ISerialPortCommunicationService, SerialPortCommunicationService>();
            services.AddSingleton<ISerialPortConnectionManager, SerialPortConnectionManager>();
            services.AddSingleton<ISerialPortCommunicationServiceFactory, SerialPortCommunicationServiceFactory>();
            services.AddScoped<ISerialPortMonitorService, SerialPortMonitorService>();
            return services;
        }


    }
}
