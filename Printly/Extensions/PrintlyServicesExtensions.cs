﻿using Microsoft.Extensions.DependencyInjection;
using Printly.Services;
using Printly.System;

namespace Printly.Extensions
{
    public static class PrintlyServicesExtensions
    {
        public static IServiceCollection AddPrintlyServices(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeService, DateTimeService>();
            services.AddSingleton<ISystemStateService, SystemStateService>();
            services.AddSingleton<ISerialPortDiscoveryService, SerialPortDiscoveryService>();
            services.AddSingleton<ISerialPortCommunicationService, SerialPortCommunicationService>();
            services.AddSingleton<ISerialPortConnectionManager, SerialPortConnectionManager>();
            services.AddScoped<ISerialPortMonitorService, SerialPortMonitorService>();
            return services;
        }


    }
}