using Moq;
using Newtonsoft.Json;
using Printly.Dto.Response;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Printly.Client.UnitTests
{
    public class PrintersClientTests
    {
        [Fact]
        public async Task GivenPrinter_AndCancellationToken_WhenCreateAsync_ThenHttpAdapterCalled_AndResponseDeserialised()
        {
            // Arrange
            var mockHttpAdapter = new Mock<IHttpAdapter<PrintersClient>>();
            var sut = new PrintersClient(mockHttpAdapter.Object);

            var request = new Dto.Request.Printer()
            {
                Name = "Bob Hoskins"
            };
            var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = new ObjectResponse<Printer>()
            {
                Success = true,
                Value = new Printer()
                {
                    Id = "Hello World"
                },
                Error = new Error()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Deserialisation test"
                }
            };

            mockHttpAdapter.Setup(x => x.PostAsync(
                It.IsAny<Uri>(),
                It.IsAny<StringContent>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json")
                });

            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var result = await sut.CreateAsync(
                request,
                cancellationTokenSource.Token);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(response.Value.Id, result.Value.Id);
            Assert.Equal(response.Error.Message, result.Error.Message);
            mockHttpAdapter.Verify(x => x.PostAsync(
                It.Is<Uri>(y => y.ToString() == new Uri($"/api/Printers", UriKind.Relative).ToString()),
                It.Is<StringContent>(y => y.ReadAsStringAsync().GetAwaiter().GetResult() == requestContent.ReadAsStringAsync().GetAwaiter().GetResult()),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenCancellationToken_WhenGetAllAsync_ThenHttpAdapterCalled_AndResponseDeserialised()
        {
            // Arrange
            var mockHttpAdapter = new Mock<IHttpAdapter<PrintersClient>>();
            var sut = new PrintersClient(mockHttpAdapter.Object);

            var response = new ObjectResponse<List<Printer>>()
            {
                Success = true,
                Value = new List<Printer>()
                {
                    new Printer()
                    {
                        Id = "Hello World"
                    }
                },
                Error = new Error()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Deserialisation test"
                }
            };

            mockHttpAdapter.Setup(x => x.GetAsync(
                It.IsAny<Uri>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(response))
                });

            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var result = await sut.GetAllAsync(
                cancellationTokenSource.Token);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(response.Value[0].Id, result.Value[0].Id);
            Assert.Equal(response.Error.Message, result.Error.Message);
            mockHttpAdapter.Verify(x => x.GetAsync(
                It.Is<Uri>(y => y.ToString() == new Uri($"/api/Printers", UriKind.Relative).ToString()),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenId_AndCancellationToken_WhenGetAsync_ThenHttpAdapterCalled_AndResponseDeserialised()
        {
            // Arrange
            var mockHttpAdapter = new Mock<IHttpAdapter<PrintersClient>>();
            var sut = new PrintersClient(mockHttpAdapter.Object);

            var response = new ObjectResponse<Printer>()
            {
                Success = true,
                Value = new Printer()
                {
                    Id = "Hello World"
                },
                Error = new Error()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Deserialisation test"
                }
            };
            var id = Guid.NewGuid().ToString();

            mockHttpAdapter.Setup(x => x.GetAsync(
                It.IsAny<Uri>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(response))
                });

            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var result = await sut.GetAsync(
                id,
                cancellationTokenSource.Token);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(response.Value.Id, result.Value.Id);
            Assert.Equal(response.Error.Message, result.Error.Message);
            mockHttpAdapter.Verify(x => x.GetAsync(
                It.Is<Uri>(y => y.ToString() == new Uri($"/api/Printers/{id}", UriKind.Relative).ToString()),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenId_AndPrinter_AndCancellationToken_WhenUpdateAsync_ThenHttpAdapterCalled_AndResponseDeserialised()
        {
            // Arrange
            var mockHttpAdapter = new Mock<IHttpAdapter<PrintersClient>>();
            var sut = new PrintersClient(mockHttpAdapter.Object);

            var request = new Dto.Request.Printer()
            {
                Name = "Bob Hoskins"
            };
            var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = new Response()
            {
                Success = true,
                Error = new Error()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Deserialisation test"
                }
            };
            var id = Guid.NewGuid().ToString();

            mockHttpAdapter.Setup(x => x.PutAsync(
                It.IsAny<Uri>(),
                It.IsAny<StringContent>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(response))
                });

            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var result = await sut.UpdateAsync(
                id,
                request,
                cancellationTokenSource.Token);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(response.Error.Message, result.Error.Message);
            mockHttpAdapter.Verify(x => x.PutAsync(
                It.Is<Uri>(y => y.ToString() == new Uri($"/api/Printers/{id}", UriKind.Relative).ToString()),
                It.Is<StringContent>(y => y.ReadAsStringAsync().GetAwaiter().GetResult() == requestContent.ReadAsStringAsync().GetAwaiter().GetResult()),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenId_AndCancellationToken_WhenDeleteAsync_ThenHttpAdapterCalled_AndResponseDeserialised()
        {
            // Arrange
            var mockHttpAdapter = new Mock<IHttpAdapter<PrintersClient>>();
            var sut = new PrintersClient(mockHttpAdapter.Object);

            var response = new Response()
            {
                Success = true,
                Error = new Error()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Deserialisation test"
                }
            };
            var id = Guid.NewGuid().ToString();

            mockHttpAdapter.Setup(x => x.DeleteAsync(
                It.IsAny<Uri>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(response))
                });

            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            var result = await sut.DeleteAsync(
                id,
                cancellationTokenSource.Token);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(response.Error.Message, result.Error.Message);
            mockHttpAdapter.Verify(x => x.DeleteAsync(
                It.Is<Uri>(y => y.ToString() == new Uri($"/api/Printers/{id}", UriKind.Relative).ToString()),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }
    }
}
