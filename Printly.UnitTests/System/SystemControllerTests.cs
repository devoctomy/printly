using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Printly.System;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Printly.UnitTests.System
{
    public class SystemControllerTests
    {
        [Fact]
        public async Task GivenNoParams_WhenGetSystemInfo_ThenGetSystemInfoCommandSent_AndResponseReturned()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var sut = new SystemController(
                mockMediator.Object,
                Mock.Of<ILogger<SystemController>>());

            var response = new GetSystemInfoResponse()
            {
                SystemInfo = new Dto.Response.SystemInfo()
                {
                    StartedAt = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0))
                }
            };

            mockMediator.Setup(x => x.Send<GetSystemInfoResponse>(
                It.IsAny<IRequest<GetSystemInfoResponse>>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(response));

            // Act
            var result = await sut.GetSystemInfo();

            // Assert
            mockMediator.Verify(x => x.Send<GetSystemInfoResponse>(
                It.IsAny<IRequest<GetSystemInfoResponse>>(),
                It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(result.Success);
            Assert.Equal(response.SystemInfo, result.Value);
        }
    }
}
