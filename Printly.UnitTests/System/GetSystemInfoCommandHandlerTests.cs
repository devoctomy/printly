using MongoDB.Bson;
using Moq;
using Printly.System;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Printly.UnitTests.System
{
    public class GetSystemInfoCommandHandlerTests
    {
        [Fact]
        public async Task GivenCommand_WhenHandled_ThenStateFromSystemStateServiceReturned()
        {
            // Arrange
            var mockSystemStateService = new Mock<ISystemStateService>();
            var sut = new GetSystemInfoCommandHandler(mockSystemStateService.Object);

            var config = new Domain.Models.Configuration()
            {
                Id = ObjectId.GenerateNewId(),
                SystemId = Guid.NewGuid().ToString()
            };
            var uptime = new TimeSpan(1, 0, 0);
            var startedAt = DateTime.UtcNow.Subtract(uptime);

            mockSystemStateService.SetupGet(x => x.Configuration)
                .Returns(config);

            mockSystemStateService.SetupGet(x => x.StartedAt)
                .Returns(startedAt);

            mockSystemStateService.SetupGet(x => x.Uptime)
                .Returns(uptime);

            // Act
            var result = await sut.Handle(
                new GetSystemInfoCommand(),
                CancellationToken.None);

            // Assert
            Assert.Equal(config.SystemId, result.SystemInfo.SystemId);
            Assert.Equal(startedAt, result.SystemInfo.StartedAt);
            Assert.Equal(uptime, result.SystemInfo.Uptime);
        }
    }
}
