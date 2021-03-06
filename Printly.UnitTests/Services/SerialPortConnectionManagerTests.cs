﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
            var serialPortConnectionManagerConfiguration = new SerialPortConnectionManagerConfiguration
            {
                DefaultBaudRate = 9600,
                DefaultDataBits = 8,
                DefaultParity = Parity.None,
                DefaultStopBits = StopBits.One,
                DefaultHandshake = Handshake.None,
                DefaultReadTimeout = new TimeSpan(0, 0, 1),
                DefaultWriteTimeout = new TimeSpan(0, 0, 1)
            };
            var mockSerialPortCommunicationServiceFactory = new Mock<ISerialPortCommunicationServiceFactory>();
            _mockSerialPortCommunicationService = new Mock<ISerialPortCommunicationService>();
            _sut = new SerialPortConnectionManager(
                mockSerialPortCommunicationServiceFactory.Object,
                serialPortConnectionManagerConfiguration,
                Mock.Of<ILogger<SerialPortConnectionManager>>());

            mockSerialPortCommunicationServiceFactory.Setup(x => x.Create())
                .Returns(_mockSerialPortCommunicationService.Object);
        }

        private void MockPortOpen(bool success)
        {
            _mockSerialPortCommunicationService.Setup(x => x.Open(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<Parity>(),
                It.IsAny<int>(),
                It.IsAny<StopBits>(),
                It.IsAny<Handshake>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>()))
                .Returns(success);
        }

        private ISerialPortCommunicationService GetOrOpen()
        {
            return _sut.GetOrOpen(
                _portName,
                _baudRate,
                _parity,
                _dataBits,
                _stopBits,
                _handshake,
                _readTimeout,
                _writeTimeout);
        }

        private ISerialPortCommunicationService GetOrOpenViaRequest()
        {
            var request = new DefaultHttpContext().Request;
            request.Path = "//terminal?portName=COM1&baudRate=9600&parity=None&dataBits=8&stopBits=One&Handshake=None";
            return _sut.GetOrOpen(request);
        }

        [Fact]
        public void GivenPortName_AndConnectionNotCached_AndConnectionAvailable_WhenGetOrOpen_ThenConnectionOpenedAndReturned()
        {
            // Arrange
            MockPortOpen(true);

            // Act
            var result = GetOrOpen();

            // Assert
            Assert.Equal(_mockSerialPortCommunicationService.Object, result);
        }

        [Fact]
        public void GivenPortSettingsAsReqyest_WhenOpen_ThenPortCreated_AndPortOpened_AndOpenStateReturned()
        {
            // Arrange
            MockPortOpen(true);

            // Act
            var result = GetOrOpenViaRequest();

            // Assert
            Assert.Equal(_mockSerialPortCommunicationService.Object, result);
        }

        [Fact]
        public void GivenPortName_AndConnectionNotCached_AndConnectionNotAvailable_WhenGetOrOpen_ThenExceptionThrown()
        {
            // Arrange
            MockPortOpen(false);

            // Act & Assert
            Assert.ThrowsAny<SerialPortConnectionException>(() =>
            {
                GetOrOpen();
            });
        }

        [Fact]
        public void GivenPortName_AndConnectionCached_AndConnectionAvailable_WhenGetOrOpen_ThenCachedConnectionReturned()
        {
            // Arrange
            MockPortOpen(true);

            GetOrOpen();

            // Act
            var result = GetOrOpen();

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
            MockPortOpen(true);
            GetOrOpen();

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
