using Printly.Services;
using System;
using System.IO.Ports;
using Xunit;

namespace Printly.UnitTests.Services
{
    public class NativeSerialPortFactoryTests
    {
        [Fact]
        public void GivenPortName_AndBaudRate_AndParity_AndDataBits_AndStopBits_AndHandshake_AndReadTimeout_AndWriteTimeout_WhenCreate_ThenNativeSerialPortReturned()
        {
            // Arrange
            var portName = "COM1";
            var baudRate = 9600;
            var parity = Parity.None;
            var dataBits = 8;
            var stopBits = StopBits.One;
            var handshake = Handshake.None;
            var readTimeout = new TimeSpan(0, 0, 1);
            var writeTimeout = new TimeSpan(0, 0, 2);
            var sut = new NativeSerialPortFactory();

            // Act
            var result = sut.Create(
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
            Assert.Equal(portName, result.PortName);
            Assert.Equal(baudRate, result.BaudRate);
            Assert.Equal(parity, result.Parity);
            Assert.Equal(dataBits, result.DataBits);
            Assert.Equal(stopBits, result.StopBits);
            Assert.Equal(handshake, result.Handshake);
            Assert.Equal(readTimeout.TotalMilliseconds, result.ReadTimeout);
            Assert.Equal(writeTimeout.TotalMilliseconds, result.WriteTimeout);
        }
    }
}
