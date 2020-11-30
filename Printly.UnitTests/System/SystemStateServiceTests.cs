using MongoDB.Bson;
using Moq;
using Printly.Domain.Models;
using Printly.Domain.Services;
using Printly.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Printly.UnitTests.System
{
    public class SystemStateServiceTests
    {
        [Fact]
        public async Task GivenExistingService_WhenInitialise_ThenSystemStateInitialisedCorrectly()
        {
            // Arrange
            var mockDateTimeService = new Mock<IDateTimeService>();
            var mockSerialPortDiscoveryService = new Mock<ISerialPortDiscoveryService>();
            var mockConfigDataService = new Mock<IDataStorageService<Configuration>>();
            var uptime = new TimeSpan(1, 0, 0);
            mockDateTimeService.SetupGet(x => x.UtcNow)
                .Returns(DateTime.UtcNow.Subtract(uptime));
            var sut = new Printly.System.SystemStateService(
                mockDateTimeService.Object,
                mockSerialPortDiscoveryService.Object,
                mockConfigDataService.Object);

            var configurations = new List<Configuration>()
            {
                new Configuration()
                {
                    Id = ObjectId.GenerateNewId(),
                    SystemId = Guid.NewGuid().ToString()
                }
            };

            mockSerialPortDiscoveryService.Setup(x => x.GetPorts())
                .Returns(new string[] { "COM1", "COM2" });

            mockConfigDataService.Setup(x => x.Get(
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(configurations);

            // Act
            await sut.InitialiseAsync(CancellationToken.None);

            // Assert
            Assert.Equal(configurations[0].Id.ToString(), sut.Configuration.Id.ToString());
            Assert.Equal(configurations[0].SystemId, sut.Configuration.SystemId);
            Assert.True(sut.Uptime - uptime < new TimeSpan(0, 1, 0));
            Assert.Equal("COM1,COM2", string.Join(',', sut.SerialPorts));
        }

        [Fact]
        public async Task GivenNewService_WhenInitialise_ThenSystemStateInitialisedCorrectly()
        {
            // Arrange
            var mockDateTimeService = new Mock<IDateTimeService>();
            var mockSerialPortDiscoveryService = new Mock<ISerialPortDiscoveryService>();
            var mockConfigDataService = new Mock<IDataStorageService<Configuration>>();
            var uptime = new TimeSpan(1, 0, 0);
            mockDateTimeService.SetupGet(x => x.UtcNow)
                .Returns(DateTime.UtcNow.Subtract(uptime));
            var sut = new Printly.System.SystemStateService(
                mockDateTimeService.Object,
                mockSerialPortDiscoveryService.Object,
                mockConfigDataService.Object);

            mockConfigDataService.Setup(x => x.Get(
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Configuration>());

            mockConfigDataService.Setup(x => x.Create(
                It.IsAny<Configuration>(),
                It.IsAny<CancellationToken>()));

            // Act
            await sut.InitialiseAsync(CancellationToken.None);

            // Assert
            Assert.True(sut.Uptime - uptime < new TimeSpan(0, 1, 0));
        }

        [Fact]
        public async Task GivenInitialisedService_WhenReset_ThenUptimeReset()
        {
            // Arrange
            var mockDateTimeService = new Mock<IDateTimeService>();
            var mockSerialPortDiscoveryService = new Mock<ISerialPortDiscoveryService>();
            var mockConfigDataService = new Mock<IDataStorageService<Configuration>>();
            var uptime = new TimeSpan(1, 0, 0);
            mockDateTimeService.SetupGet(x => x.UtcNow)
                .Returns(DateTime.UtcNow.Subtract(uptime));
            var sut = new Printly.System.SystemStateService(
                mockDateTimeService.Object,
                mockSerialPortDiscoveryService.Object,
                mockConfigDataService.Object);

            mockConfigDataService.Setup(x => x.Get(
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Configuration>());

            mockConfigDataService.Setup(x => x.Create(
                It.IsAny<Configuration>(),
                It.IsAny<CancellationToken>()));

            await sut.InitialiseAsync(CancellationToken.None);
            await Task.Delay(new TimeSpan(0, 0, 5));

            // Act
            sut.Reset();

            // Assert
            Assert.True(sut.Uptime - uptime < new TimeSpan(0, 1, 0));
        }


    }
}
