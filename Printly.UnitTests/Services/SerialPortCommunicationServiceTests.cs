using Microsoft.Extensions.Logging;
using Moq;
using Printly.Services;
using System;
using System.IO.Ports;
using Xunit;

namespace Printly.UnitTests.Services
{
    public class SerialPortCommunicationServiceTests
    {
        private Mock<ISerialPortFactory> _mockSerialPortFactory;
        private Mock<ISerialPort> _mockSerialPort;
        private TestableSerialPort _testableSerialPort;
        private SerialPortCommunicationService _sut;
        private readonly string _portName = "Bob";
        private readonly int _baudRate = 100;
        private readonly Parity _parity = Parity.Mark;
        private readonly int _dataBits = 89;
        private readonly StopBits _stopBits = StopBits.OnePointFive;
        private readonly Handshake _handshake = Handshake.RequestToSendXOnXOff;
        private readonly TimeSpan _readTimeout = new TimeSpan(0, 0, 30);
        private readonly TimeSpan _writeTimeout = new TimeSpan(0, 0, 31);

        private void SetupForMockSerialPort()
        {
            _mockSerialPortFactory = new Mock<ISerialPortFactory>();
            _mockSerialPort = new Mock<ISerialPort>();
            _sut = new SerialPortCommunicationService(
                _mockSerialPortFactory.Object,
                Mock.Of<ILogger<SerialPortCommunicationService>>());

            _mockSerialPortFactory.Setup(x => x.Create(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<Parity>(),
                It.IsAny<int>(),
                It.IsAny<StopBits>(),
                It.IsAny<Handshake>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
                .Returns(_mockSerialPort.Object);
        }

        private void SetupForTestableSerialPort()
        {
            _mockSerialPortFactory = new Mock<ISerialPortFactory>();
            _testableSerialPort = new TestableSerialPort();
            _sut = new SerialPortCommunicationService(
                _mockSerialPortFactory.Object,
                Mock.Of<ILogger<SerialPortCommunicationService>>());

            _mockSerialPortFactory.Setup(x => x.Create(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<Parity>(),
                It.IsAny<int>(),
                It.IsAny<StopBits>(),
                It.IsAny<Handshake>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
                .Returns(_testableSerialPort);
        }

        [Fact]
        public void GivenPortName_AndBaudRate_AndParty_AndDataBits_AndStopBits_AndHandshake_AndReadTimeout_AndWriteTimeout_WhenOpen_ThenPortCreated_AndPortOpened_AndOpenStateReturned()
        {
            // Arrange
            SetupForMockSerialPort();
            _mockSerialPort.Setup(x => x.Open());
            _mockSerialPort.SetupGet(x => x.IsOpen).Returns(true);

            // Act
            var result = _sut.Open(
                _portName,
                _baudRate,
                _parity,
                _dataBits,
                _stopBits,
                _handshake,
                _readTimeout,
                _writeTimeout);

            // Assert
            Assert.True(result);
            _mockSerialPortFactory.Verify(x => x.Create(
                It.Is<string>(y => y == _portName),
                It.Is<int>(y => y == _baudRate),
                It.Is<Parity>(y => y == _parity),
                It.Is<int>(y => y == _dataBits),
                It.Is<StopBits>(y => y == _stopBits),
                It.Is<Handshake>(y => y == _handshake),
                It.Is<TimeSpan>(y => y == _readTimeout),
                It.Is<TimeSpan>(y => y == _writeTimeout)), Times.Once);
        }

        [Fact]
        public void GivenNotOpenPort_WhenClose_ThenPortNotClosed()
        {
            // Arrange
            SetupForMockSerialPort();

            // Act
            _sut.Close();

            // Assert
            _mockSerialPort.Verify(x => x.Close(), Times.Never);
        }

        [Fact]
        public void GivenOpenPort_WhenClose_ThenPortClosed()
        {
            // Arrange
            SetupForMockSerialPort();
            _mockSerialPort.Setup(x => x.Open());
            _mockSerialPort.SetupGet(x => x.IsOpen).Returns(true);
            _sut.Open(
                _portName,
                _baudRate,
                _parity,
                _dataBits,
                _stopBits,
                _handshake,
                _readTimeout,
                _writeTimeout);

            // Act
            _sut.Close();

            // Assert
            _mockSerialPort.Verify(x => x.Close(), Times.Once);
        }

        [Fact]
        public void GivenOpenPort_WhenWrite_ThenPortWrite()
        {
            // Arrange
            SetupForMockSerialPort();
            _mockSerialPort.Setup(x => x.Open());
            _mockSerialPort.SetupGet(x => x.IsOpen).Returns(true);
            _sut.Open(
                _portName,
                _baudRate,
                _parity,
                _dataBits,
                _stopBits,
                _handshake,
                _readTimeout,
                _writeTimeout);

            // Act
            _sut.Write(
                null,
                0,
                0);

            // Assert
            _mockSerialPort.Verify(x => x.Write(
                It.IsAny<byte[]>(),
                It.IsAny<int>(),
                It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenOpenPort_WhenReceivedData_ThenEventRaised()
        {
            // Arrange
            SetupForTestableSerialPort();

            _sut.Open(
                _portName,
                _baudRate,
                _parity,
                _dataBits,
                _stopBits,
                _handshake,
                _readTimeout,
                _writeTimeout);

            var eventRaised = false;
            _sut.DataReceived += (object sender, SerialDataReceivedEventArgs e) =>
            {
                eventRaised = true;
            };

            // Act
            _testableSerialPort.TestDataReceived(null);

            // Assert
            Assert.True(eventRaised);
        }

        [Fact]
        public void GivenOpenPort_WhenErrorData_ThenEventRaised()
        {
            // Arrange
            SetupForTestableSerialPort();

            _sut.Open(
                _portName,
                _baudRate,
                _parity,
                _dataBits,
                _stopBits,
                _handshake,
                _readTimeout,
                _writeTimeout);

            var eventRaised = false;
            _sut.ErrorReceived += (object sender, SerialErrorReceivedEventArgs e) =>
            {
                eventRaised = true;
            };

            // Act
            _testableSerialPort.TestErrorReceived(null);

            // Assert
            Assert.True(eventRaised);
        }
    }
}
