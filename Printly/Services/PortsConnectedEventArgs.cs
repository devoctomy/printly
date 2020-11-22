using System;

namespace Printly.Services
{
    public class PortsConnectedEventArgs : EventArgs
    {
        public string[] SerialPorts { get; set; }
    }
}
