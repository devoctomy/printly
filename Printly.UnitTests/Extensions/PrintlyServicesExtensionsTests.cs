﻿using Microsoft.Extensions.DependencyInjection;
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
        public void GivenConfig_WhenAddPrintlyClient_ThenClientAddedForInjection()
        {
            // Arrange
            var sut = new ServiceCollection();

            // Act
            sut.AddPrintlyServices();
            sut.AddPrintlyDataServices(new Domain.Services.MongoDbConfiguration()
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "Test"
            });
            var serviceProvider = sut.BuildServiceProvider();

            // Assert
            Assert.NotNull(serviceProvider.GetServices<IDateTimeService>());
            Assert.NotNull(serviceProvider.GetServices<ISystemStateService>());
            Assert.NotNull(serviceProvider.GetServices<ISerialPortDiscoveryService>());
            Assert.NotNull(serviceProvider.GetServices<ISerialPortCommunicationService>());
            Assert.NotNull(serviceProvider.GetServices<ISerialPortConnectionManager>());
            Assert.NotNull(serviceProvider.GetServices<ISerialPortMonitorService>());
        }
    }
}