using Microsoft.AspNetCore.Http;
using Moq;
using Printly.Middleware;
using Printly.Services;
using Printly.Terminal;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Printly.UnitTests.Middleware
{
    public class TerminalMiddlewareTests
    {
        [Fact]
        public async Task GivenHttpContext_AndSerialPortMonitorService_AndRequestPathNotCorrect_WhenInvoke_ThenNextInvoked()
        {
            // Arrange
            var mockSerialPortConnectionManager = new Mock<ISerialPortConnectionManager>();
            var mockWebSocketConnectionService = new Mock<IWebSocketConnectionService>();
            var mockWebSocketTerminalService = new Mock<IWebSocketTerminalService>();
            var mockRequestDelegate = new Mock<RequestDelegate>();
            var sut = new TerminalMiddleware(
                mockRequestDelegate.Object,
                mockSerialPortConnectionManager.Object,
                mockWebSocketConnectionService.Object);
            var mockSerialPortMonitorService = new Mock<ISerialPortMonitorService>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpContext.SetupGet(x => x.Request)
                .Returns(mockHttpRequest.Object);
            mockHttpRequest.SetupGet(x => x.Path)
                .Returns("/bob");
            mockWebSocketConnectionService.Setup(x => x.Create(
                It.IsAny<HttpContext>()))
                .ReturnsAsync(mockWebSocketTerminalService.Object);
            mockRequestDelegate.Setup(x => x.Invoke(
                It.IsAny<HttpContext>()));

            // Act
            await sut.Invoke(
                mockHttpContext.Object,
                mockSerialPortMonitorService.Object);

            // Assert
            mockRequestDelegate.Verify(x => x.Invoke(
                It.IsAny<HttpContext>()), Times.Once);
        }

        [Fact]
        public async Task GivenHttpContext_AndSerialPortMonitorService_AndRequestCorrect_WhenInvoke_ThenWebSocketConnectionEstablished_AndSerialPortOpened_AndSerialPortMonitorServiceStarted_AndCommsLoopRunAndSerialPortMonitorServiceStopped_AndSerialPortClosed()
        {
            // Arrange
            var mockSerialPortConnectionManager = new Mock<ISerialPortConnectionManager>();
            var mockSerialPortCommunicationService = new Mock<ISerialPortCommunicationService>();
            var mockWebSocketConnectionService = new Mock<IWebSocketConnectionService>();
            var mockWebSocketTerminalService = new Mock<IWebSocketTerminalService>();
            var mockRequestDelegate = new Mock<RequestDelegate>();
            var mockSerialPortMonitorService = new Mock<ISerialPortMonitorService>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockWebSocket = new Mock<WebSocket>();
            var mockWebSocketManager = new Mock<WebSocketManager>();
            var mockHttpRequest = new Mock<HttpRequest>();
            var sut = new TerminalMiddleware(
                mockRequestDelegate.Object,
                mockSerialPortConnectionManager.Object,
                mockWebSocketConnectionService.Object);
            mockWebSocket.Setup(x => x.SendAsync(
                It.IsAny<ArraySegment<byte>>(),
                It.IsAny<WebSocketMessageType>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true));
            mockWebSocketManager.Setup(x => x.AcceptWebSocketAsync())
                .Returns(Task.FromResult(mockWebSocket.Object));
            mockWebSocketManager.SetupGet(x => x.IsWebSocketRequest)
                .Returns(true);
            mockHttpContext.SetupGet(x => x.WebSockets)
                .Returns(mockWebSocketManager.Object);
            mockHttpContext.SetupGet(x => x.Request)
                .Returns(mockHttpRequest.Object);
            mockHttpRequest.SetupGet(x => x.Path)
                .Returns("/terminal?portName=COM1&baudRate=9600&parity=None&dataBits=8&stopBits=One&Handshake=None");
            mockWebSocketConnectionService.Setup(x => x.Create(
                It.IsAny<HttpContext>()))
                .ReturnsAsync(mockWebSocketTerminalService.Object);
            mockRequestDelegate.Setup(x => x.Invoke(
                It.IsAny<HttpContext>()));

            mockSerialPortCommunicationService.SetupGet(x => x.PortName)
                .Returns("Bob");

            mockSerialPortConnectionManager.Setup(x => x.GetOrOpen(
                It.IsAny<HttpRequest>()))
                .Returns(mockSerialPortCommunicationService.Object);

            // Act
            await sut.Invoke(
                mockHttpContext.Object,
                mockSerialPortMonitorService.Object);

            // Assert
            mockWebSocketConnectionService.Verify(x => x.Create(
                It.IsAny<HttpContext>()), Times.Once);
            mockSerialPortConnectionManager.Verify(x => x.GetOrOpen(
                It.IsAny<HttpRequest>()), Times.Once);
            mockSerialPortMonitorService.Verify(x => x.Start(), Times.Once);
            mockWebSocketTerminalService.Verify(x => x.RunCommsLoopAsync(
                It.Is<ISerialPortCommunicationService>(y => y == mockSerialPortCommunicationService.Object),
                It.IsAny<CancellationToken>()), Times.Once);
            mockSerialPortMonitorService.Verify(x => x.Stop(), Times.Once);
            mockSerialPortConnectionManager.Verify(x => x.Close(
                It.Is<string>(y => y == "Bob")), Times.Once);
        }
    }
}
