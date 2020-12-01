using Printly.Services;
using System;
using System.IO.Ports;

namespace Printly.UnitTests.Services
{
    public class TestableSerialPort : ISerialPort
    {
        public event SerialDataReceivedEventHandler DataReceived;
        public event SerialErrorReceivedEventHandler ErrorReceived;

        public bool IsOpen { get; private set; }

        public string PortName { get; }

        public int BaudRate { get; }

        public Parity Parity { get; }

        public int DataBits { get; }

        public StopBits StopBits { get; }

        public Handshake Handshake { get; }

        public int ReadTimeout { get; }

        public int WriteTimeout { get; }

        public void Close()
        {
            IsOpen = false;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Open()
        {
            IsOpen = true;
        }

        public string ReadExisting()
        {
            return string.Empty;
        }

        public void Write(
            byte[] data,
            int offset,
            int count)
        {
        }

        public void TestDataReceived(SerialDataReceivedEventArgs args)
        {
            DataReceived?.Invoke(
                this,
                args);
        }

        public void TestErrorReceived(SerialErrorReceivedEventArgs args)
        {
            ErrorReceived?.Invoke(
                this,
                args);
        }
    }
}
