using System;
using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;

namespace Printly.Services
{
    public class NativeSerialPort : ISerialPort
    {
        public event SerialDataReceivedEventHandler DataReceived;
        public event SerialErrorReceivedEventHandler ErrorReceived;

        public bool IsOpen { get; private set; }
        public string PortName => InnerPort.PortName;
        public int BaudRate => InnerPort.BaudRate;
        public Parity Parity => InnerPort.Parity;
        public int DataBits => InnerPort.DataBits;
        public StopBits StopBits => InnerPort.StopBits;
        public Handshake Handshake => InnerPort.Handshake;
        public int ReadTimeout => InnerPort.ReadTimeout;
        public int WriteTimeout => InnerPort.WriteTimeout;
        public SerialPort InnerPort { get; private set; }

        private bool _disposed;

        public NativeSerialPort(
            string portName,
            int baudRate,
            Parity parity,
            int dataBits,
            StopBits stopBits,
            Handshake handshake,
            TimeSpan readTimeout,
            TimeSpan writeTimeout)
        {
            InnerPort = new SerialPort(
                portName,
                baudRate,
                parity,
                dataBits,
                stopBits)
            {
                Handshake = handshake,
                ReadTimeout = (int)readTimeout.TotalMilliseconds,
                WriteTimeout = (int)writeTimeout.TotalMilliseconds
            };
            InnerPort.DataReceived += InnerPort_DataReceived;
            InnerPort.ErrorReceived += InnerPort_ErrorReceived;
        }

        [ExcludeFromCodeCoverage]
        ~NativeSerialPort()
        {
            Dispose(false);
        }

        [ExcludeFromCodeCoverage]
        private void InnerPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            DataReceived?.Invoke(
                this,
                e);
        }

        [ExcludeFromCodeCoverage]
        private void InnerPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            ErrorReceived?.Invoke(
                this,
                e);
        }

        [ExcludeFromCodeCoverage]
        public void Close()
        {
            InnerPort.Close();
            IsOpen = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                InnerPort.Dispose();
            }

            _disposed = true;
        }

        [ExcludeFromCodeCoverage]
        public void Open()
        {
            InnerPort.Open();
            IsOpen = true;
        }

        [ExcludeFromCodeCoverage]
        public string ReadExisting()
        {
            return InnerPort.ReadExisting();
        }

        [ExcludeFromCodeCoverage]
        public void Write(
            byte[] data,
            int offset,
            int count)
        {
            InnerPort.Write(
                data,
                offset,
                count);
        }
    }
}
