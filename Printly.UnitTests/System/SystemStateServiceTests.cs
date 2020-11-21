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
            var uptime = new TimeSpan(1, 0, 0);
            mockDateTimeService.SetupGet(x => x.UtcNow)
                .Returns(DateTime.UtcNow.Subtract(uptime));
            var sut = new SystemStateService(mockDateTimeService.Object);

            // Act & Assert
            Assert.True(sut.Uptime - uptime < new TimeSpan(0, 1, 0));
        }
    }
}
