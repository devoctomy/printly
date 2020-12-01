using Moq;
using Printly.Exceptions;
using Printly.Services;
using System;
using System.IO.Ports;
using Xunit;

namespace Printly.UnitTests.Services
{
    public class SerialPortConnectionManagerTests
    {
        [Fact]
        public void GivenPortName_AndConnectionNotCached_AndConnectionAvailable_WhenGetOrOpen_ThenConnectionOpenedAndReturned()
        {
            // Arrange
            var mockSerialPortCommunicationServiceFactory = new Mock<ISerialPortCommunicationServiceFactory>();
            var mockSerialPortCommunicationService = new Mock<ISerialPortCommunicationService>();
            var sut = new SerialPortConnectionManager(mockSerialPortCommunicationServiceFactory.Object);

            var portName = "Bob";
            var baudRate = 9600;
            var parity = Parity.None;
            var dataBits = 8;
            var stopBits = StopBits.One;
            var handshake = Handshake.None;
            var readTimeout = new TimeSpan(0, 1, 0);
            var writeTimeout = new TimeSpan(0, 1, 0);

            mockSerialPortCommunicationServiceFactory.Setup(x => x.Create())
                .Returns(mockSerialPortCommunicationService.Object);

            mockSerialPortCommunicationService.Setup(x => x.Open(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<Parity>(),
                It.IsAny<int>(),
                It.IsAny<StopBits>(),
                It.IsAny<Handshake>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
                .Returns(true);

            // Act
            var result = sut.GetOrOpen(
                portName,
                baudRate,
                parity,
                dataBits,
                stopBits,
                handshake,
                readTimeout,
                writeTimeout);

            // Assert
            Assert.Equal(mockSerialPortCommunicationService.Object, result);
        }

        [Fact]
        public void GivenPortName_AndConnectionNotCached_AndConnectionNotAvailable_WhenGetOrOpen_ThenExceptionThrown()
        {
            // Arrange
            var mockSerialPortCommunicationServiceFactory = new Mock<ISerialPortCommunicationServiceFactory>();
            var mockSerialPortCommunicationService = new Mock<ISerialPortCommunicationService>();
            var sut = new SerialPortConnectionManager(mockSerialPortCommunicationServiceFactory.Object);

            var portName = "Bob";
            var baudRate = 9600;
            var parity = Parity.None;
            var dataBits = 8;
            var stopBits = StopBits.One;
            var handshake = Handshake.None;
            var readTimeout = new TimeSpan(0, 1, 0);
            var writeTimeout = new TimeSpan(0, 1, 0);

            mockSerialPortCommunicationServiceFactory.Setup(x => x.Create())
                .Returns(mockSerialPortCommunicationService.Object);

            mockSerialPortCommunicationService.Setup(x => x.Open(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<Parity>(),
                It.IsAny<int>(),
                It.IsAny<StopBits>(),
                It.IsAny<Handshake>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
                .Returns(false);

            // Act & Assert
            Assert.ThrowsAny<SerialPortConnectionException>(() =>
            {
                sut.GetOrOpen(
                    portName,
                    baudRate,
                    parity,
                    dataBits,
                    stopBits,
                    handshake,
                    readTimeout,
                    writeTimeout);
            });
        }

        [Fact]
        public void GivenPortName_AndConnectionCached_AndConnectionAvailable_WhenGetOrOpen_ThenCachedConnectionReturned()
        {
            // Arrange
            var mockSerialPortCommunicationServiceFactory = new Mock<ISerialPortCommunicationServiceFactory>();
            var mockSerialPortCommunicationService = new Mock<ISerialPortCommunicationService>();
            var sut = new SerialPortConnectionManager(mockSerialPortCommunicationServiceFactory.Object);

            var portName = "Bob";
            var baudRate = 9600;
            var parity = Parity.None;
            var dataBits = 8;
            var stopBits = StopBits.One;
            var handshake = Handshake.None;
            var readTimeout = new TimeSpan(0, 1, 0);
            var writeTimeout = new TimeSpan(0, 1, 0);

            mockSerialPortCommunicationServiceFactory.Setup(x => x.Create())
                .Returns(mockSerialPortCommunicationService.Object);

            mockSerialPortCommunicationService.Setup(x => x.Open(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<Parity>(),
                It.IsAny<int>(),
                It.IsAny<StopBits>(),
                It.IsAny<Handshake>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
                .Returns(true);

            sut.GetOrOpen(
                portName,
                baudRate,
                parity,
                dataBits,
                stopBits,
                handshake,
                readTimeout,
                writeTimeout);

            // Act
            var result = sut.GetOrOpen(
                    portName,
                    baudRate,
                    parity,
                    dataBits,
                    stopBits,
                    handshake,
                    readTimeout,
                    writeTimeout);

            // Assert
            Assert.NotNull(result);
            mockSerialPortCommunicationService.Verify(x => x.Open(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<Parity>(),
                It.IsAny<int>(),
                It.IsAny<StopBits>(),
                It.IsAny<Handshake>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()), Times.Once);
        }

        [Fact]
        public void GivenPortName_AndConnectionOpened_WhenClose_ThenTrueReturned()
        {
            // Arrange
            var mockSerialPortCommunicationServiceFactory = new Mock<ISerialPortCommunicationServiceFactory>();
            var mockSerialPortCommunicationService = new Mock<ISerialPortCommunicationService>();
            var sut = new SerialPortConnectionManager(mockSerialPortCommunicationServiceFactory.Object);

            var portName = "Bob";
            var baudRate = 9600;
            var parity = Parity.None;
            var dataBits = 8;
            var stopBits = StopBits.One;
            var handshake = Handshake.None;
            var readTimeout = new TimeSpan(0, 1, 0);
            var writeTimeout = new TimeSpan(0, 1, 0);

            mockSerialPortCommunicationServiceFactory.Setup(x => x.Create())
                .Returns(mockSerialPortCommunicationService.Object);

            mockSerialPortCommunicationService.Setup(x => x.Open(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<Parity>(),
                It.IsAny<int>(),
                It.IsAny<StopBits>(),
                It.IsAny<Handshake>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
                .Returns(true);

            sut.GetOrOpen(
                portName,
                baudRate,
                parity,
                dataBits,
                stopBits,
                handshake,
                readTimeout,
                writeTimeout);

            // Act
            var result = sut.Close(portName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GivenPortName_AndConnectionNotOpened_WhenClose_ThenFalseReturned()
        {
            // Arrange
            var mockSerialPortCommunicationServiceFactory = new Mock<ISerialPortCommunicationServiceFactory>();
            var mockSerialPortCommunicationService = new Mock<ISerialPortCommunicationService>();
            var sut = new SerialPortConnectionManager(mockSerialPortCommunicationServiceFactory.Object);
            var portName = "Bob";

            mockSerialPortCommunicationServiceFactory.Setup(x => x.Create())
                .Returns(mockSerialPortCommunicationService.Object);

            // Act
            var result = sut.Close(portName);

            // Assert
            Assert.False(result);
        }
    }
}
