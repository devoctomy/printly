using Microsoft.Extensions.DependencyInjection;
using Printly.Services;
using Printly.System;
using Printly.Terminal;
using System;

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

            services.AddSingleton<SerialPortConnectionManagerConfiguration>(config =>
            {
                return new SerialPortConnectionManagerConfiguration
                {
                    DefaultBaudRate = appSettings.DefaultTerminalBaudRate,
                    DefaultDataBits = appSettings.DefaultTerminalDataBits,
                    DefaultStopBits = appSettings.DefaultTerminalStopBits,
                    DefaultParity = appSettings.DefaultTerminalParity,
                    DefaultHandshake = appSettings.DefaultTerminalHandshake,
                    DefaultReadTimeout = new TimeSpan(0, 0, 0, 0, appSettings.DefaultReadTimeoutMilliseconds),
                    DefaultWriteTimeout = new TimeSpan(0, 0, 0, 0, appSettings.DefaultWriteTimeoutMilliseconds),
                };
            });

            services.AddSingleton<WebSocketTerminalServiceConfiguration>(config =>
            {
                return new WebSocketTerminalServiceConfiguration
                {
                    ReceiveBufferSize = appSettings.TerminalReadBufferSize
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
            services.AddSingleton<IWebSocketConnectionService, WebSocketConnectionService>();
            services.AddSingleton<IWebSocketTerminalService, WebSocketTerminalService>();
            services.AddScoped<IIdValidator, PrinterIdValidator>();
            return services;
        }
    }
}
