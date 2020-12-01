using Moq;
using Printly.Services;
using System;
using System.IO.Ports;
using Xunit;

namespace Printly.UnitTests.Services
{
    public class SerialPortCommunicationServiceTests
    {
        [Fact]
        public void GivenPortName_AndBaudRate_AndParty_AndDataBits_AndStopBits_AndHandshake_AndReadTimeout_AndWriteTimeout_WhenOpen_ThenPortCreated_AndPortOpened_AndOpenStateReturned()
        {
            // Arrange
            var mockSerialPortFactory = new Mock<ISerialPortFactory>();
            var mockSerialPort = new Mock<ISerialPort>();
            var sut = new SerialPortCommunicationService(mockSerialPortFactory.Object);

            var portName = "Bob";
            var baudRate = 100;
            var parity = Parity.Mark;
            var dataBits = 89;
            var stopBits = StopBits.OnePointFive;
            var handshake = Handshake.RequestToSendXOnXOff;
            var readTimeout = new TimeSpan(0, 0, 30);
            var writeTimeout = new TimeSpan(0, 0, 31);

            mockSerialPortFactory.Setup(x => x.Create(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<Parity>(),
                It.IsAny<int>(),
                It.IsAny<StopBits>(),
                It.IsAny<Handshake>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
                .Returns(mockSerialPort.Object);

            mockSerialPort.Setup(x => x.Open());
            mockSerialPort.SetupGet(x => x.IsOpen).Returns(true);

            // Act
            var result = sut.Open(
                portName,
                baudRate,
                parity,
                dataBits,
                stopBits,
                handshake,
                readTimeout,
                writeTimeout);

            // Assert
            Assert.True(result);
            mockSerialPortFactory.Verify(x => x.Create(
                It.Is<string>(y => y == portName),
                It.Is<int>(y => y == baudRate),
                It.Is<Parity>(y => y == parity),
                It.Is<int>(y => y == dataBits),
                It.Is<StopBits>(y => y == stopBits),
                It.Is<Handshake>(y => y == handshake),
                It.Is<TimeSpan>(y => y == readTimeout),
                It.Is<TimeSpan>(y => y == writeTimeout)), Times.Once);
        }

        [Fact]
        public void GivenNotOpenPort_WhenClose_ThenPortNotClosed()
        {
            // Arrange
            var mockSerialPortFactory = new Mock<ISerialPortFactory>();
            var mockSerialPort = new Mock<ISerialPort>();
            var sut = new SerialPortCommunicationService(mockSerialPortFactory.Object);

            // Act
            sut.Close();

            // Assert
            mockSerialPort.Verify(x => x.Close(), Times.Never);
        }

        [Fact]
        public void GivenOpenPort_WhenClose_ThenPortClosed()
        {
            // Arrange
            var mockSerialPortFactory = new Mock<ISerialPortFactory>();
            var mockSerialPort = new Mock<ISerialPort>();
            var sut = new SerialPortCommunicationService(mockSerialPortFactory.Object);

            var portName = "Bob";
            var baudRate = 100;
            var parity = Parity.Mark;
            var dataBits = 89;
            var stopBits = StopBits.OnePointFive;
            var handshake = Handshake.RequestToSendXOnXOff;
            var readTimeout = new TimeSpan(0, 0, 30);
            var writeTimeout = new TimeSpan(0, 0, 31);

            mockSerialPortFactory.Setup(x => x.Create(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<Parity>(),
                It.IsAny<int>(),
                It.IsAny<StopBits>(),
                It.IsAny<Handshake>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
                .Returns(mockSerialPort.Object);

            mockSerialPort.Setup(x => x.Open());
            mockSerialPort.SetupGet(x => x.IsOpen).Returns(true);
            sut.Open(
                portName,
                baudRate,
                parity,
                dataBits,
                stopBits,
                handshake,
                readTimeout,
                writeTimeout);

            // Act
            sut.Close();

            // Assert
            mockSerialPort.Verify(x => x.Close(), Times.Once);
        }

        [Fact]
        public void GivenOpenPort_WhenWrite_ThenPortWrite()
        {
            // Arrange
            var mockSerialPortFactory = new Mock<ISerialPortFactory>();
            var mockSerialPort = new Mock<ISerialPort>();
            var sut = new SerialPortCommunicationService(mockSerialPortFactory.Object);

            var portName = "Bob";
            var baudRate = 100;
            var parity = Parity.Mark;
            var dataBits = 89;
            var stopBits = StopBits.OnePointFive;
            var handshake = Handshake.RequestToSendXOnXOff;
            var readTimeout = new TimeSpan(0, 0, 30);
            var writeTimeout = new TimeSpan(0, 0, 31);

            mockSerialPortFactory.Setup(x => x.Create(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<Parity>(),
                It.IsAny<int>(),
                It.IsAny<StopBits>(),
                It.IsAny<Handshake>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
                .Returns(mockSerialPort.Object);

            mockSerialPort.Setup(x => x.Open());
            mockSerialPort.SetupGet(x => x.IsOpen).Returns(true);
            sut.Open(
                portName,
                baudRate,
                parity,
                dataBits,
                stopBits,
                handshake,
                readTimeout,
                writeTimeout);

            // Act
            sut.Write(
                null,
                0,
                0);

            // Assert
            mockSerialPort.Verify(x => x.Write(
                It.IsAny<byte[]>(),
                It.IsAny<int>(),
                It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GivenOpenPort_WhenReceivedData_ThenEventRaised()
        {
            // Arrange
            var mockSerialPortFactory = new Mock<ISerialPortFactory>();
            var testableSerialPort = new TestableSerialPort();
            var sut = new SerialPortCommunicationService(mockSerialPortFactory.Object);

            var portName = "Bob";
            var baudRate = 100;
            var parity = Parity.Mark;
            var dataBits = 89;
            var stopBits = StopBits.OnePointFive;
            var handshake = Handshake.RequestToSendXOnXOff;
            var readTimeout = new TimeSpan(0, 0, 30);
            var writeTimeout = new TimeSpan(0, 0, 31);

            mockSerialPortFactory.Setup(x => x.Create(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<Parity>(),
                It.IsAny<int>(),
                It.IsAny<StopBits>(),
                It.IsAny<Handshake>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
                .Returns(testableSerialPort);

            sut.Open(
                portName,
                baudRate,
                parity,
                dataBits,
                stopBits,
                handshake,
                readTimeout,
                writeTimeout);

            var eventRaised = false;
            sut.DataReceived += (object sender, SerialDataReceivedEventArgs e) =>
            {
                eventRaised = true;
            };

            // Act
            testableSerialPort.TestDataReceived(null);

            // Assert
            Assert.True(eventRaised);
        }

        [Fact]
        public void GivenOpenPort_WhenErrorData_ThenEventRaised()
        {
            // Arrange
            var mockSerialPortFactory = new Mock<ISerialPortFactory>();
            var testableSerialPort = new TestableSerialPort();
            var sut = new SerialPortCommunicationService(mockSerialPortFactory.Object);

            var portName = "Bob";
            var baudRate = 100;
            var parity = Parity.Mark;
            var dataBits = 89;
            var stopBits = StopBits.OnePointFive;
            var handshake = Handshake.RequestToSendXOnXOff;
            var readTimeout = new TimeSpan(0, 0, 30);
            var writeTimeout = new TimeSpan(0, 0, 31);

            mockSerialPortFactory.Setup(x => x.Create(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<Parity>(),
                It.IsAny<int>(),
                It.IsAny<StopBits>(),
                It.IsAny<Handshake>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
                .Returns(testableSerialPort);

            sut.Open(
                portName,
                baudRate,
                parity,
                dataBits,
                stopBits,
                handshake,
                readTimeout,
                writeTimeout);

            var eventRaised = false;
            sut.ErrorReceived += (object sender, SerialErrorReceivedEventArgs e) =>
            {
                eventRaised = true;
            };

            // Act
            testableSerialPort.TestErrorReceived(null);

            // Assert
            Assert.True(eventRaised);
        }
    }
}
