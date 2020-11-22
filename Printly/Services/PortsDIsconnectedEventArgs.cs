using System;

namespace Printly.Services
{
    public class PortsDisconnectedEventArgs : EventArgs
    {
        public string[] SerialPorts { get; set; }
    }
}
