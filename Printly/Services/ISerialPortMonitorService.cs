using System;

namespace Printly.Services
{
    public interface ISerialPortMonitorService
    {
        event EventHandler<PortsConnectedEventArgs> PortsConnected;
        event EventHandler<PortsDisconnectedEventArgs> PortsDisconnected;

        void Start();
        void Stop();
    }
}
