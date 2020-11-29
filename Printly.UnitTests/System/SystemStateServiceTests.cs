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
        public async Task GivenService_WhenInitialise_ThenSystemStateInitialisedCorrectly()
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

            await sut.InitialiseAsync(CancellationToken.None);

            // Act & Assert
            Assert.Equal(configurations[0].Id.ToString(), sut.Configuration.Id.ToString());
            Assert.Equal(configurations[0].SystemId, sut.Configuration.SystemId);
            Assert.True(sut.Uptime - uptime < new TimeSpan(0, 1, 0));
            Assert.Equal("COM1,COM2", string.Join(',', sut.SerialPorts));
        }
    }
}
