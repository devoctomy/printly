using System.IO.Ports;

namespace Printly
{
    public class AppSettings
    {
        public string MongoDbStorageConnectionString { get; set; }
        public string MongoDbStorageDatabaseName { get; set; }
        public int SerialPortPollPauseMilliseconds { get; set; }
        public int TerminalReadBufferSize { get; set; }
        public int DefaultTerminalBaudRate { get; set; }
        public int DefaultTerminalDataBits { get; set; }
        public StopBits DefaultTerminalStopBits { get; set; }
        public Parity DefaultTerminalParity { get; set; }
        public Handshake DefaultTerminalHandshake { get; set; }
        public int DefaultReadTimeoutMilliseconds { get; set; }
        public int DefaultWriteTimeoutMilliseconds { get; set; }
    }
}
