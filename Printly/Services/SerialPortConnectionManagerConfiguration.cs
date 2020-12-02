using System;
using System.IO.Ports;

namespace Printly.Services
{
    public class SerialPortConnectionManagerConfiguration
    {
        public int DefaultBaudRate { get; set; }
        public Parity DefaultParity { get; set; }
        public int DefaultDataBits { get; set; }
        public StopBits DefaultStopBits { get; set; }
        public Handshake DefaultHandshake { get; set; }
        public TimeSpan DefaultReadTimeout { get; set; }
        public TimeSpan DefaultWriteTimeout { get; set; }
    }
}
