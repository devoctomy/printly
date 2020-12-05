using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Printly.Middleware;
using Printly.Services;
using Printly.Terminal;
using System;
using System.IO.Ports;
using System.Net;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
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
                mockWebSocketConnectionService.Object,
                Mock.Of<ILogger<TerminalMiddleware>>());
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
                mockWebSocketConnectionService.Object,
                Mock.Of<ILogger<TerminalMiddleware>>());
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
                .Returns("COM1");

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
                It.Is<string>(y => y == "COM1")), Times.Once);
        }

        [Fact]
        public async Task GivenHttpContext_AndSerialPortMonitorService_AndRequestNotCorrect_WhenInvoke_ThenBadRequestStatusCodeSet_AndFunctionReturns()
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
            var mockHttpResponse = new Mock<HttpResponse>();
            var sut = new TerminalMiddleware(
                mockRequestDelegate.Object,
                mockSerialPortConnectionManager.Object,
                mockWebSocketConnectionService.Object,
                Mock.Of<ILogger<TerminalMiddleware>>());
            mockWebSocketManager.SetupGet(x => x.IsWebSocketRequest)
                .Returns(true);
            mockHttpContext.SetupGet(x => x.WebSockets)
                .Returns(mockWebSocketManager.Object);
            mockHttpContext.SetupGet(x => x.Request)
                .Returns(mockHttpRequest.Object);
            mockHttpRequest.SetupGet(x => x.Path)
                .Returns("/terminal?portName=COM1&baudRate=9600&parity=None&dataBits=8&stopBits=One&Handshake=None");
            mockRequestDelegate.Setup(x => x.Invoke(
                It.IsAny<HttpContext>()));

            mockHttpContext.SetupGet(x => x.Response)
                .Returns(mockHttpResponse.Object);

            var httpStatsCode = 0;
            mockHttpResponse.SetupSet(x => x.StatusCode = It.IsAny<int>())
                .Callback<int>(y => httpStatsCode = y);

            // Act
            await sut.Invoke(
                mockHttpContext.Object,
                mockSerialPortMonitorService.Object);
            Assert.Equal((int)HttpStatusCode.BadRequest, httpStatsCode);
        }

        [Fact]
        public async Task GivenHttpContext_AndSerialPortMonitorService_AndRequestCorrect_AndPortDisconnectedDuringCommsLoop_WhenInvoke_ThenRXData_ThenPortDisconnected_AndTaskCancelled()
        {
            //Arrange
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
            var mockSerialPort = new Mock<ISerialPort>();
            var sut = new TerminalMiddleware(
                mockRequestDelegate.Object,
                mockSerialPortConnectionManager.Object,
                mockWebSocketConnectionService.Object,
                Mock.Of<ILogger<TerminalMiddleware>>());
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
                .Returns("COM1");

            mockSerialPortConnectionManager.Setup(x => x.GetOrOpen(
                It.IsAny<HttpRequest>()))
                .Returns(mockSerialPortCommunicationService.Object);

            mockWebSocketTerminalService.Setup(x => x.RunCommsLoopAsync(
                It.IsAny<ISerialPortCommunicationService>(),
                It.IsAny<CancellationToken>()))
                .Returns((ISerialPortCommunicationService commsService, CancellationToken ct) =>
                {
                    return Task.Delay(50000, ct);
                });

            mockSerialPortCommunicationService.SetupGet(x => x.State)
                .Returns(mockWebSocket.Object);

            mockSerialPortCommunicationService.SetupGet(x => x.SerialPort)
                .Returns(mockSerialPort.Object);

            mockSerialPort.SetupGet(x => x.Encoding)
                .Returns(Encoding.ASCII);

            var dataString = "Hello World!";
            var dataBytes = Encoding.ASCII.GetBytes(dataString);
            var dataBuffer = new ArraySegment<byte>(dataBytes, 0, dataBytes.Length);
            mockSerialPort.Setup(x => x.ReadExisting())
                .Returns(dataString);

            var constructor = typeof(SerialDataReceivedEventArgs).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(SerialData) }, null);
            var eventArgs = (SerialDataReceivedEventArgs)constructor.Invoke(new object[] { SerialData.Chars });

            // Act
            var invokeTask = sut.Invoke(
                mockHttpContext.Object,
                mockSerialPortMonitorService.Object);
            await Task.Delay(500).ConfigureAwait(false);
            mockSerialPortCommunicationService.Raise(x => x.DataReceived += null, new object[] { mockSerialPortCommunicationService.Object, eventArgs });
            await Task.Delay(500).ConfigureAwait(false);
            mockWebSocket.Verify(x => x.SendAsync(
                It.Is<ArraySegment<byte>>(y => Convert.ToBase64String(y.Array) == Convert.ToBase64String(dataBuffer.Array)),
                It.Is<WebSocketMessageType>(y => y == WebSocketMessageType.Text),
                It.Is<bool>(y => y == true),
                It.Is<CancellationToken>(y => y == CancellationToken.None)), Times.Once);
            mockSerialPortMonitorService.Raise(x => x.PortsDisconnected += null, new PortsDisconnectedEventArgs() { SerialPorts = new string[] { "COM1" } });
            await Task.Delay(500).ConfigureAwait(false);
            Assert.True(invokeTask.IsCanceled);
        }
    }
}
