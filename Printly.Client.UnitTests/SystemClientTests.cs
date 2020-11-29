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
            var mockHttpAddapter = new Mock<IHttpAdapter<SystemClient>>();
            var sut = new SystemClient(mockHttpAddapter.Object);

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

            mockHttpAddapter.Setup(x => x.GetAsync(
                It.IsAny<Uri>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new System.Net.Http.HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(response))
                });

            // Act
            var result = await sut.GetInfoAsync(CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(startedAt, result.Value.StartedAt);
            Assert.Equal(new TimeSpan(1, 0, 0), result.Value.Uptime);
        }
    }
}
