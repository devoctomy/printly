using System;
using System.IO.Ports;

namespace Printly.Services
{
    public interface ISerialPortFactory
    {
        ISerialPort Create(
            string portName,
            int baudRate,
            Parity parity,
            int dataBits,
            StopBits stopBits,
            Handshake handshake,
            TimeSpan readTimeout,
            TimeSpan writeTimeout);
    }
}
