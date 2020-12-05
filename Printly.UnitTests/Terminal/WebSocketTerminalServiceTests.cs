using Microsoft.Extensions.Logging;
using Moq;
using Printly.Services;
using Printly.Terminal;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Printly.UnitTests.Terminal
{
    public class WebSocketTerminalServiceTests
    {
        [Fact]
        public async Task GivenSerialPortCommunicationService_AndCancellationToken_WhenRunCommsLoopAsync_ThenDataReceivedFromWebSocket_AndWrittenToSerialPort()
        {
            // Arrange
            var webSocketTerminalServiceConfiguration = new WebSocketTerminalServiceConfiguration
            {
                ReceiveBufferSize = 1024
            };
            var sut = new WebSocketTerminalService(
                webSocketTerminalServiceConfiguration,
                Mock.Of<ILogger<WebSocketTerminalService>>());
            var mockSerialPortCommunicationService = new Mock<ISerialPortCommunicationService>();
            var mockWebSocket = new Mock<WebSocket>();
            var mockSerialPort = new Mock<ISerialPort>();
            var cancellationTokenSource = new CancellationTokenSource();

            var readFromSocket = false;
            mockWebSocket.Setup(x => x.ReceiveAsync(
                It.IsAny<ArraySegment<byte>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync((ArraySegment<byte> data, CancellationToken ct) =>
                {
                    var result = new WebSocketReceiveResult(
                        0,
                        WebSocketMessageType.Text,
                        readFromSocket,
                        readFromSocket ? WebSocketCloseStatus.NormalClosure : null,
                        null);
                    readFromSocket = true;
                    return result;
                });

            mockSerialPortCommunicationService.Setup(x => x.Write(
                It.IsAny<byte[]>(),
                It.IsAny<int>(),
                It.IsAny<int>()));

            mockSerialPortCommunicationService.SetupGet(x => x.SerialPort)
                .Returns(mockSerialPort.Object);

            mockSerialPort.SetupGet(x => x.Encoding)
                .Returns(Encoding.ASCII);

            sut.WebSocket = mockWebSocket.Object;

            // Act
            await sut.RunCommsLoopAsync(
                mockSerialPortCommunicationService.Object,
                cancellationTokenSource.Token);

            // Assert
            mockWebSocket.Verify(x => x.ReceiveAsync(
                It.IsAny<ArraySegment<byte>>(),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
            mockSerialPortCommunicationService.Verify(x => x.Write(
                It.IsAny<byte[]>(),
                It.IsAny<int>(),
                It.IsAny<int>()), Times.Once);
        }
    }
}
