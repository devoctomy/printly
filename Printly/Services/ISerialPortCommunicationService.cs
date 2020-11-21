using System;
using System.IO.Ports;

namespace Printly.Services
{
    public interface ISerialPortCommunicationService
    {
        event EventHandler<SerialDataReceivedEventArgs> DataReceived;

        string PortName { get; }

        bool Open(
            string portName,
            int baudRate,
            Parity parity,
            int dataBits,
            StopBits stopBits,
            Handshake handshake,
            TimeSpan readTimeout,
            TimeSpan writeTimeout);

        void Close();

        void Write(
            byte[] buffer,
            int offset,
            int count);
    }
}
