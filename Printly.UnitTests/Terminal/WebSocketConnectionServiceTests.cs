using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Printly.Terminal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Printly.UnitTests.Terminal
{
    public class WebSocketConnectionServiceTests
    {
        [Fact]
        public async Task GivenHttpContext_AndIsWebSocketRequest_WhenCreate_ThenWebSocketAccepted_AndWebSocketTerminalServiceReturned()
        {
            // Arrange
            var webSocketTerminalServiceConfiguration = new WebSocketTerminalServiceConfiguration
            {
                ReceiveBufferSize = 1024
            };
            var sut = new WebSocketConnectionService(
                webSocketTerminalServiceConfiguration,
                Mock.Of<ILogger<WebSocketTerminalService>>());
            var mockHttpContext = new Mock<HttpContext>();
            var mockWebSocket = new Mock<WebSocket>();
            mockWebSocket.Setup(x => x.SendAsync(
                It.IsAny<ArraySegment<byte>>(),
                It.IsAny<WebSocketMessageType>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true));
            var mockWebSocketManager = new Mock<WebSocketManager>();
            mockWebSocketManager
                .Setup(x => x.AcceptWebSocketAsync())
                .Returns(Task.FromResult(mockWebSocket.Object));
            mockWebSocketManager.SetupGet(x => x.IsWebSocketRequest)
                .Returns(true);
            mockHttpContext.SetupGet(x => x.WebSockets)
                .Returns(mockWebSocketManager.Object);

            // Act
            var result = await sut.Create(mockHttpContext.Object);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GivenHttpContext_AndNotWebSocketRequest_WhenCreate_ThenNullReturned()
        {
            // Arrange
            var webSocketTerminalServiceConfiguration = new WebSocketTerminalServiceConfiguration
            {
                ReceiveBufferSize = 1024
            };
            var sut = new WebSocketConnectionService(
                webSocketTerminalServiceConfiguration,
                Mock.Of<ILogger<WebSocketTerminalService>>());
            var mockHttpContext = new Mock<HttpContext>();
            var mockWebSocketManager = new Mock<WebSocketManager>();
            mockWebSocketManager.SetupGet(x => x.IsWebSocketRequest)
                .Returns(false);
            mockHttpContext.SetupGet(x => x.WebSockets)
                .Returns(mockWebSocketManager.Object);

            // Act
            var result = await sut.Create(mockHttpContext.Object);

            // Assert
            Assert.Null(result);
        }
    }
}
