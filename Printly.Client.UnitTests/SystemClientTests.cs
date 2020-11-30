using Moq;
using Newtonsoft.Json;
using Printly.Dto.Response;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Printly.Client.UnitTests
{
    public class SystemClientTests
    {
        [Fact]
        public async Task GivenApiUp_WhenGetInfoAsync_ThenApiCalledAndResponseReturned()
        {
            // Arrange
            var mockHttpAdapter = new Mock<IHttpAdapter<SystemClient>>();
            var sut = new SystemClient(mockHttpAdapter.Object);

            var startedAt = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0));
            var response = new ObjectResponse<SystemInfo>()
            {
                Success = true,
                Value = new SystemInfo()
                {
                    StartedAt = startedAt,
                    Uptime = new TimeSpan(1, 0, 0)
                }
            };

            mockHttpAdapter.Setup(x => x.GetAsync(
                It.IsAny<Uri>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new System.Net.Http.HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(response))
                });

            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var result = await sut.GetInfoAsync(cancellationTokenSource.Token);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(startedAt, result.Value.StartedAt);
            Assert.Equal(new TimeSpan(1, 0, 0), result.Value.Uptime);
            mockHttpAdapter.Verify(x => x.GetAsync(
                It.Is<Uri>(y => y.ToString() == new Uri("/api/System", UriKind.Relative).ToString()),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }
    }
}
