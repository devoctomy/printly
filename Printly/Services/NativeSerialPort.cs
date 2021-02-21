using System;
using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Services
{
    public class NativeSerialPort : ISerialPort
    {
        public event EventHandler<SerialPortDataReceivedEventArgs> DataReceived;
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
        public Encoding Encoding => InnerPort.Encoding;

        private bool _disposed;
        private Task _reading;

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
            InnerPort.DtrEnable = true;
            InnerPort.ErrorReceived += InnerPort_ErrorReceived;
        }

        [ExcludeFromCodeCoverage]
        ~NativeSerialPort()
        {
            Dispose(false);
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
            if (_disposed)
            {
                return;
            }

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
            _reading = BeginReadingAsync(InnerPort);
        }

        [ExcludeFromCodeCoverage]
        public async Task Write(
            byte[] data,
            int offset,
            int count)
        {
            InnerPort.Write(
                data,
                offset,
                count);
            await InnerPort.BaseStream.FlushAsync();
        }

        private async Task BeginReadingAsync(SerialPort port)
        {
            await Task.Yield();
            Console.WriteLine("Started async reading from serial port.");

            var rxBuffer = new byte[1024];
            while (true)
            {
                var bytesRead = await port.BaseStream.ReadAsync(
                    rxBuffer,
                    0,
                    rxBuffer.Length,
                    CancellationToken.None);
                if (bytesRead > 0)
                {
                    var destBuffer = new byte[bytesRead];
                    Array.Copy(rxBuffer, destBuffer, bytesRead);
                    DataReceived?.Invoke(
                        this,
                        new SerialPortDataReceivedEventArgs
                        {
                            SerialPort = this,
                            Data = destBuffer
                        });
                }
            }
        }
    }
}
