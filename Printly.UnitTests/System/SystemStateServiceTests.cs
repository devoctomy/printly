using Moq;
using Printly.Services;
using Printly.System;
using System;
using Xunit;

namespace Printly.UnitTests.System
{
    public class SystemStateServiceTests
    {
        [Fact]
        public void GivenService_WhenGetUptime_ThenCorrectUptimeReturned()
        {
            // Arrange
            var mockDateTimeService = new Mock<IDateTimeService>();
            var mockSerialPortDiscoveryService = new Mock<ISerialPortDiscoveryService>();
            var uptime = new TimeSpan(1, 0, 0);
            mockDateTimeService.SetupGet(x => x.UtcNow)
                .Returns(DateTime.UtcNow.Subtract(uptime));
            var sut = new SystemStateService(
                mockDateTimeService.Object,
                mockSerialPortDiscoveryService.Object);

            mockSerialPortDiscoveryService.Setup(x => x.GetPorts())
                .Returns(new string[] { "COM1", "COM2" });

            // Act & Assert
            Assert.True(sut.Uptime - uptime < new TimeSpan(0, 1, 0));
            Assert.Equal("COM1,COM2", string.Join(',', sut.SerialPorts));
        }
    }
}
