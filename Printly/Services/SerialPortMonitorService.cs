using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Services
{
    public class SerialPortMonitorService : ISerialPortMonitorService, IDisposable
    {
        public event EventHandler<PortsConnectedEventArgs> PortsConnected;
        public event EventHandler<PortsDisconnectedEventArgs> PortsDisconnected;

        private readonly SerialPortMonitorServiceConfiguration _serialPortMonitorServiceConfiguration;
        private readonly ISerialPortDiscoveryService _serialPortDiscoveryService;
        private readonly IDateTimeService _dateTimeService;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly List<SerialPortConnectionInfo> _lastDetectedPorts = new List<SerialPortConnectionInfo>();
        private Task _monitoringTask;
        private bool _disposed;

        public SerialPortMonitorService(
            SerialPortMonitorServiceConfiguration serialPortMonitorServiceConfiguration,
            ISerialPortDiscoveryService serialPortDiscoveryService,
            IDateTimeService dateTimeService)
        {
            _serialPortMonitorServiceConfiguration = serialPortMonitorServiceConfiguration;
            _serialPortDiscoveryService = serialPortDiscoveryService;
            _dateTimeService = dateTimeService;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        ~SerialPortMonitorService() => Dispose(false);

        public void Start()
        {
            if(_monitoringTask == null)
            {
                _monitoringTask = Monitor(_cancellationTokenSource.Token);
            }
        }

        public async Task Stop()
        {
            _cancellationTokenSource.Cancel();
            await _monitoringTask.ConfigureAwait(false);
            _monitoringTask = null;
        }

        private async Task Monitor(CancellationToken cancellationToken)
        {
            await Task.Yield();
            while(!cancellationToken.IsCancellationRequested)
            {
                var ports = _serialPortDiscoveryService.GetPorts();
                var connectedPorts = ports.Where(x => !_lastDetectedPorts.Any(y => y.PortName == x)).ToArray();
                var disconnectedPorts = _lastDetectedPorts.Where(x => !ports.Contains(x.PortName)).Select(y => y.PortName).ToArray();

                if(connectedPorts.Length > 0)
                {
                    foreach (var curConnected in connectedPorts)
                    {
                        _lastDetectedPorts.Add(new SerialPortConnectionInfo
                        {
                            PortName = curConnected,
                            OnlineSince = _dateTimeService.UtcNow
                        });
                    }

                    PortsConnected?.Invoke(
                        this,
                        new PortsConnectedEventArgs
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
                        new PortsDisconnectedEventArgs
                        {
                            SerialPorts = disconnectedPorts
                        });
                }

                await Task.Delay(
                    _serialPortMonitorServiceConfiguration.PollPauseMilliseconds,
                    cancellationToken).ConfigureAwait(false);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if(_disposed)
            {
                return;
            }

            if(disposing)
            {
                _cancellationTokenSource.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
