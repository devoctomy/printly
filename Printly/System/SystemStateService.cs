using Printly.Domain.Models;
using Printly.Domain.Services;
using Printly.Domain.Services.System;
using Printly.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.System
{
    public class SystemStateService : ISystemStateService
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly ISerialPortDiscoveryService _serialPortDiscoveryService;
        private readonly IDataStorageService<Configuration> _configurationDataStorageService;
        private DateTime _startedAt;

        public Configuration Configuration { get; set; }
        public DateTime StartedAt => _startedAt;
        public TimeSpan Uptime => DateTime.UtcNow - StartedAt;
        public string[] SerialPorts => _serialPortDiscoveryService.GetPorts();

        public SystemStateService(
            IDateTimeService dateTimeService,
            ISerialPortDiscoveryService serialPortDiscoveryService,
            IDataStorageService<Configuration> configurationDataStorageService)
        {
            _dateTimeService = dateTimeService;
            _serialPortDiscoveryService = serialPortDiscoveryService;
            _configurationDataStorageService = configurationDataStorageService;
        }

        public async Task InitialiseAsync(CancellationToken cancellationToken)
        {
            var systemInfo = await _configurationDataStorageService.Get(cancellationToken);
            if (systemInfo.Count == 0)
            {
                var newSystemInfo = new Configuration();
                await _configurationDataStorageService.Create(
                    newSystemInfo,
                    cancellationToken);
                Configuration = newSystemInfo;
            }
            else
            {
                Configuration = systemInfo[0];
            }

            Reset();
        }

        public void Reset()
        {
            _startedAt = _dateTimeService.UtcNow;
        }
    }
}
