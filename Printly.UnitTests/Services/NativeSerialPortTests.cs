using Printly.Services;
using System;
using System.IO.Ports;
using Xunit;

namespace Printly.UnitTests.Services
{
    public class NativeSerialPortTests
    {
        [Fact]
        public void GivenNativeSerialPort_WhenDispose_ThenInnerPortDisposed()
        {
            // Arrange
            var sut = new NativeSerialPort(
                "COM1",
                9600,
                Parity.None,
                8,
                StopBits.One,
                Handshake.None,
                new TimeSpan(0, 0, 5),
                new TimeSpan(0, 0, 5));
            var disposed = false;
            sut.InnerPort.Disposed += (object sender, EventArgs e) =>
            {
                disposed = true;
            };

            // Act
            sut.Dispose();

            // Assert
            Assert.True(disposed);
        }

        [Fact]
        public void GivenNativeSerialPort_WhenDisposeTwice_ThenInnerPortDisposedOnce()
        {
            // Arrange
            var sut = new NativeSerialPort(
                "COM1",
                9600,
                Parity.None,
                8,
                StopBits.One,
                Handshake.None,
                new TimeSpan(0, 0, 5),
                new TimeSpan(0, 0, 5));
            var disposeCount = 0;
            sut.InnerPort.Disposed += (object sender, EventArgs e) =>
            {
                disposeCount += 1;
            };

            // Act
            sut.Dispose();
            sut.Dispose();

            // Assert
            Assert.Equal(1, disposeCount);
        }
    }
}
