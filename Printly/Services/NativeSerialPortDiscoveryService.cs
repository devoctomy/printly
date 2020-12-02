using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;

namespace Printly.Services
{
    [ExcludeFromCodeCoverage]
    public class NativeSerialPortDiscoveryService : ISerialPortDiscoveryService
    {
        public string[] GetPorts()
        {
            return SerialPort.GetPortNames();
        }
    }
}
