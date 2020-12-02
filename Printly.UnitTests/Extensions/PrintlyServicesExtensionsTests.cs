using Microsoft.Extensions.DependencyInjection;
using Printly.Domain.Services.Extensions;
using Printly.Extensions;
using Printly.Services;
using Printly.System;
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
                SerialPortPollPauseMilliseconds = 100
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
            Assert.NotNull(serviceProvider.GetService<IExceptionHandlerService>());
            Assert.NotNull(serviceProvider.GetService<IDateTimeService>());
            Assert.NotNull(serviceProvider.GetService<ISystemStateService>());
            Assert.NotNull(serviceProvider.GetService<ISerialPortFactory>());
            Assert.NotNull(serviceProvider.GetService<ISerialPortDiscoveryService>());
            Assert.NotNull(serviceProvider.GetService<ISerialPortCommunicationService>());
            Assert.NotNull(serviceProvider.GetService<ISerialPortConnectionManager>());
            Assert.NotNull(serviceProvider.GetService<ISerialPortMonitorService>());
        }
    }
}
