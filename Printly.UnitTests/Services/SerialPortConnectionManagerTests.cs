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
        private readonly Mock<ISerialPortCommunicationServiceFactory>  _mockSerialPortCommunicationServiceFactory;
        private readonly Mock<ISerialPortCommunicationService> _mockSerialPortCommunicationService;
        private readonly SerialPortConnectionManager _sut;
        private readonly string _portName = "Bob";
        private readonly int _baudRate = 9600;
        private readonly Parity _parity = Parity.None;
        private readonly int _dataBits = 8;
        private readonly StopBits _stopBits = StopBits.One;
        private readonly Handshake _handshake = Handshake.None;
        private readonly TimeSpan _readTimeout = new TimeSpan(0, 1, 0);
        private readonly TimeSpan _writeTimeout = new TimeSpan(0, 1, 0);

        public SerialPortConnectionManagerTests()
        {
            _mockSerialPortCommunicationServiceFactory = new Mock<ISerialPortCommunicationServiceFactory>();
            _mockSerialPortCommunicationService = new Mock<ISerialPortCommunicationService>();
            _sut = new SerialPortConnectionManager(_mockSerialPortCommunicationServiceFactory.Object);

            _mockSerialPortCommunicationServiceFactory.Setup(x => x.Create())
                .Returns(_mockSerialPortCommunicationService.Object);
        }

        [Fact]
        public void GivenPortName_AndConnectionNotCached_AndConnectionAvailable_WhenGetOrOpen_ThenConnectionOpenedAndReturned()
        {
            // Arrange
            _mockSerialPortCommunicationService.Setup(x => x.Open(
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
            var result = _sut.GetOrOpen(
                _portName,
                _baudRate,
                _parity,
                _dataBits,
                _stopBits,
                _handshake,
                _readTimeout,
                _writeTimeout);

            // Assert
            Assert.Equal(_mockSerialPortCommunicationService.Object, result);
        }

        [Fact]
        public void GivenPortName_AndConnectionNotCached_AndConnectionNotAvailable_WhenGetOrOpen_ThenExceptionThrown()
        {
            // Arrange
            _mockSerialPortCommunicationService.Setup(x => x.Open(
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
                _sut.GetOrOpen(
                    _portName,
                    _baudRate,
                    _parity,
                    _dataBits,
                    _stopBits,
                    _handshake,
                    _readTimeout,
                    _writeTimeout);
            });
        }

        [Fact]
        public void GivenPortName_AndConnectionCached_AndConnectionAvailable_WhenGetOrOpen_ThenCachedConnectionReturned()
        {
            // Arrange
            _mockSerialPortCommunicationService.Setup(x => x.Open(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<Parity>(),
                It.IsAny<int>(),
                It.IsAny<StopBits>(),
                It.IsAny<Handshake>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
                .Returns(true);

            _sut.GetOrOpen(
                _portName,
                _baudRate,
                _parity,
                _dataBits,
                _stopBits,
                _handshake,
                _readTimeout,
                _writeTimeout);

            // Act
            var result = _sut.GetOrOpen(
                    _portName,
                    _baudRate,
                    _parity,
                    _dataBits,
                    _stopBits,
                    _handshake,
                    _readTimeout,
                    _writeTimeout);

            // Assert
            Assert.NotNull(result);
            _mockSerialPortCommunicationService.Verify(x => x.Open(
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
            _mockSerialPortCommunicationService.Setup(x => x.Open(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<Parity>(),
                It.IsAny<int>(),
                It.IsAny<StopBits>(),
                It.IsAny<Handshake>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
                .Returns(true);

            _sut.GetOrOpen(
                _portName,
                _baudRate,
                _parity,
                _dataBits,
                _stopBits,
                _handshake,
                _readTimeout,
                _writeTimeout);

            // Act
            var result = _sut.Close(_portName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GivenPortName_AndConnectionNotOpened_WhenClose_ThenFalseReturned()
        {
            // Arrange
            // Nothing to arrange

            // Act
            var result = _sut.Close(_portName);

            // Assert
            Assert.False(result);
        }
    }
}
