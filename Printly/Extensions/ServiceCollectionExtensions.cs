using Microsoft.Extensions.DependencyInjection;
using Printly.Services;
using Printly.System;

namespace Printly.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPrintlyServices(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeService, DateTimeService>();
            services.AddSingleton<ISystemStateService, SystemStateService>();
            services.AddSingleton<ISerialPortDiscoveryService, SerialPortDiscoveryService>();
            services.AddSingleton<ISerialPortCommunicationService, SerialPortCommunicationService>();
            return services;
        }


    }
}
