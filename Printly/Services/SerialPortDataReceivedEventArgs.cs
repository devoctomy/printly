using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Printly.Services
{
    public class SerialPortDataReceivedEventArgs
    {
        public ISerialPort SerialPort { get; set; }
        public byte[] Data { get; set; }
    }
}
