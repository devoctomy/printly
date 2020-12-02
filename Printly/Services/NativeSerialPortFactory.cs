using System;
using System.IO.Ports;

namespace Printly.Services
{
    public class NativeSerialPortFactory : ISerialPortFactory
    {
        public ISerialPort Create(
            string portName,
            int baudRate,
            Parity parity,
            int dataBits,
            StopBits stopBits,
            Handshake handshake,
            TimeSpan readTimeout,
            TimeSpan writeTimeout)
        {
            return new NativeSerialPort(
                portName,
                baudRate,
                parity,
                dataBits,
                stopBits,
                handshake,
                readTimeout,
                writeTimeout);
        }
    }
}
