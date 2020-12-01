using Printly.Services;
using System;
using Xunit;

namespace Printly.UnitTests.Services
{
    public class DateTimeServiceTests
    {
        [Fact]
        public void Given_WhenUtcNow_ThenUtcNowReturned()
        {
            // Arrange
            var sut = new DateTimeService();

            // Act
            var before = DateTime.UtcNow;
            var result = sut.UtcNow;
            var after = DateTime.UtcNow;

            // Assert
            Assert.True(result > before && result < after);
        }

    }
}
