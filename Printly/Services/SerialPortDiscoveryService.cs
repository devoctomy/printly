using System.IO.Ports;

namespace Printly.Services
{
    public class SerialPortDiscoveryService : ISerialPortDiscoveryService
    {
        public string[] GetPorts()
        {
            return SerialPort.GetPortNames();
        }
    }
}
