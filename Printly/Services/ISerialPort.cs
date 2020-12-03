using System;
using System.IO.Ports;
using System.Text;

namespace Printly.Services
{
    public interface ISerialPort : IDisposable
    {
        event SerialDataReceivedEventHandler DataReceived;
        event SerialErrorReceivedEventHandler ErrorReceived;
        bool IsOpen { get; }
        string PortName { get; }
        int BaudRate { get; }
        Parity Parity { get; }
        int DataBits { get; }
        StopBits StopBits { get; }
        Handshake Handshake { get; }
        int ReadTimeout { get; }
        int WriteTimeout { get; }
        Encoding Encoding { get; }
        void Open();
        void Close();
        void Write(
            byte[] data,
            int offset,
            int count);
        string ReadExisting();
    }
}
