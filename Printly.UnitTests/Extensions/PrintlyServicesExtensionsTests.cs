using Microsoft.Extensions.DependencyInjection;
using Printly.Domain.Services.Extensions;
using Printly.Extensions;
using Printly.Services;
using Printly.System;
using Printly.Terminal;
using System.IO.Ports;
using Xunit;

namespace Printly.UnitTests.Extensions
{
    public class PrintlyServicesExtensionsTests
    {
        [Fact]
        public void GivenConfig_WhenAddPrintlyServices_ThenServicesAddedForInjection()
        {
            // Arrange
            var appSettings = new AppSettings
            {
                SerialPortPollPauseMilliseconds = 100,
                DefaultTerminalBaudRate = 9600,
                DefaultTerminalDataBits = 8,
                DefaultTerminalStopBits = StopBits.One,
                DefaultTerminalParity = Parity.None,
                DefaultTerminalHandshake = Handshake.None,
                DefaultReadTimeoutMilliseconds = 1000,
                DefaultWriteTimeoutMilliseconds = 2000
            };
            var sut = new ServiceCollection();

            // Act
            sut.AddPrintlyServices(appSettings);
            sut.AddPrintlyDomainServices(new Domain.Services.MongoDbConfiguration
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "Test"
            });
            var serviceProvider = sut.BuildServiceProvider();

            // Assert
            var serialPortMonitorServiceConfiguration = serviceProvider.GetService<SerialPortMonitorServiceConfiguration>();
            Assert.NotNull(serialPortMonitorServiceConfiguration);
            Assert.Equal(appSettings.SerialPortPollPauseMilliseconds, serialPortMonitorServiceConfiguration.PollPauseMilliseconds);
            
            var serialPortConnectionManagerConfiguration = serviceProvider.GetService<SerialPortConnectionManagerConfiguration>();
            Assert.NotNull(serialPortConnectionManagerConfiguration);
            Assert.Equal(appSettings.DefaultTerminalBaudRate, serialPortConnectionManagerConfiguration.DefaultBaudRate);
            Assert.Equal(appSettings.DefaultTerminalDataBits, serialPortConnectionManagerConfiguration.DefaultDataBits);
            Assert.Equal(appSettings.DefaultTerminalStopBits, serialPortConnectionManagerConfiguration.DefaultStopBits);
            Assert.Equal(appSettings.DefaultTerminalParity, serialPortConnectionManagerConfiguration.DefaultParity);
            Assert.Equal(appSettings.DefaultTerminalHandshake, serialPortConnectionManagerConfiguration.DefaultHandshake);
            Assert.Equal(appSettings.DefaultReadTimeoutMilliseconds, serialPortConnectionManagerConfiguration.DefaultReadTimeout.TotalMilliseconds);
            Assert.Equal(appSettings.DefaultWriteTimeoutMilliseconds, serialPortConnectionManagerConfiguration.DefaultWriteTimeout.TotalMilliseconds);
            
            Assert.NotNull(serviceProvider.GetService<IExceptionHandlerService>());
            Assert.NotNull(serviceProvider.GetService<IDateTimeService>());
            Assert.NotNull(serviceProvider.GetService<ISystemStateService>());
            Assert.NotNull(serviceProvider.GetService<ISerialPortFactory>());
            Assert.NotNull(serviceProvider.GetService<ISerialPortDiscoveryService>());
            Assert.NotNull(serviceProvider.GetService<ISerialPortCommunicationService>());
            Assert.NotNull(serviceProvider.GetService<ISerialPortConnectionManager>());
            Assert.NotNull(serviceProvider.GetService<ISerialPortMonitorService>());
            Assert.NotNull(serviceProvider.GetService<IWebSocketConnectionService>());
            Assert.NotNull(serviceProvider.GetService<IWebSocketTerminalService>());
        }
    }
}
