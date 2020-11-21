﻿using System;
using System.IO.Ports;

namespace Printly.Services
{
    public interface ISerialPortConnectionManager
    {
        ISerialPortCommunicationService Get(string portName);

        ISerialPortCommunicationService GetOrOpen(
            string portName,
            int baudRate,
            Parity parity,
            int dataBits,
            StopBits stopBits,
            Handshake handshake,
            TimeSpan readTimeout,
            TimeSpan writeTimeout);

        void Close(string portName);
    }
}