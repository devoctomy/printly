using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using Printly.Dto.Response;
using Printly.Services;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Printly.UnitTests.Services
{
    public class ExceptionHandlerServiceTests
    {
        [Fact]
        public async Task GivenHttpContext_WhenHandle_ThenErrorResponseReturned_AndStatus500()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var mockExceptionHandlerFeature = new Mock<IExceptionHandlerFeature>();
            var sut = new ExceptionHandlerService();
            var cancellationTokenSource = new CancellationTokenSource();
            var message = "Hello World!";

            mockExceptionHandlerFeature.SetupGet(x => x.Error)
                .Returns(new Exception(message));

            httpContext.Response.Body = new MemoryStream();
            httpContext.Features.Set(mockExceptionHandlerFeature.Object);

            // Act
            await sut.HandleAsync(
                httpContext,
                cancellationTokenSource.Token);

            // Assert
            mockExceptionHandlerFeature.VerifyGet(x => x.Error, Times.Once);
            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseJson = Encoding.UTF8.GetString(((MemoryStream)httpContext.Response.Body).ToArray());
            var response = JsonConvert.DeserializeObject<Response>(responseJson);
            Assert.False(response.Success);
            Assert.Equal(HttpStatusCode.InternalServerError, response.Error.HttpStatusCode);
            Assert.Equal(message, response.Error.Message);
        }
    }
}
