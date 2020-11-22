using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Services
{
    public class SerialPortMonitorService : ISerialPortMonitorService
    {
        public event EventHandler<PortsConnectedEventArgs> PortsConnected;
        public event EventHandler<PortsDisconnectedEventArgs> PortsDisconnected;

        private ISerialPortDiscoveryService _serialPortDiscoveryService;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _monitoringTask;
        private List<SerialPortConnectionInfo> _lastDetectedPorts = new List<SerialPortConnectionInfo>();


        public SerialPortMonitorService(ISerialPortDiscoveryService serialPortDiscoveryService)
        {
            _serialPortDiscoveryService = serialPortDiscoveryService;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            if(_monitoringTask == null)
            {
                _monitoringTask = Monitor(_cancellationTokenSource.Token);
            }
        }

        public async void Stop()
        {
            _cancellationTokenSource.Cancel();
            await _monitoringTask;
            _monitoringTask = null;
        }

        private async Task Monitor(CancellationToken cancellationToken)
        {
            await Task.Yield();
            while(!cancellationToken.IsCancellationRequested)
            {
                var ports = _serialPortDiscoveryService.GetPorts();
                var stablePorts = ports.Where(x => _lastDetectedPorts.Any(y => y.PortName == x)).ToArray();
                var connectedPorts = ports.Where(x => !_lastDetectedPorts.Any(y => y.PortName == x)).ToArray();
                var disconnectedPorts = _lastDetectedPorts.Where(x => !ports.Contains(x.PortName)).Select(y => y.PortName).ToArray();

                if(connectedPorts.Length > 0)
                {
                    foreach (var curConnected in connectedPorts)
                    {
                        _lastDetectedPorts.Add(new SerialPortConnectionInfo()
                        {
                            PortName = curConnected,
                            OnlineSince = DateTime.UtcNow
                        });
                    }

                    PortsConnected?.Invoke(
                        this,
                        new PortsConnectedEventArgs()
                        {
                            SerialPorts = connectedPorts
                        });
                }

                if(disconnectedPorts.Length > 0)
                {
                    foreach(var curDisconnected in disconnectedPorts)
                    {
                        var info = _lastDetectedPorts.SingleOrDefault(x => x.PortName == curDisconnected);
                        _lastDetectedPorts.Remove(info);
                    }

                    PortsDisconnected?.Invoke(
                        this,
                        new PortsDisconnectedEventArgs()
                        {
                            SerialPorts = disconnectedPorts
                        });
                }

                await Task.Delay(5000);
            }
        }
    }
}
