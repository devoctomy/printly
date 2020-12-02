using System;
using System.Threading.Tasks;

namespace Printly.Services
{
    public interface ISerialPortMonitorService
    {
        event EventHandler<PortsConnectedEventArgs> PortsConnected;
        event EventHandler<PortsDisconnectedEventArgs> PortsDisconnected;

        bool IsRunning { get; }
        void Start();
        Task Stop();
    }
}
