using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Printly.Services
{
    public interface ISerialPortDiscoveryService
    {
        string[] GetPorts();
    }
}
