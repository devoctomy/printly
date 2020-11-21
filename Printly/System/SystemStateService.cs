using Printly.Services;
using System;

namespace Printly.System
{
    public class SystemStateService : ISystemStateService
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly ISerialPortDiscoveryService _serialPortDiscoveryService;
        private DateTime _startedAt;

        public DateTime StartedAt => _startedAt;
        public TimeSpan Uptime => DateTime.UtcNow - StartedAt;
        public string[] SerialPorts => _serialPortDiscoveryService.GetPorts();

        public SystemStateService(
            IDateTimeService dateTimeService,
            ISerialPortDiscoveryService serialPortDiscoveryService)
        {
            _dateTimeService = dateTimeService;
            _serialPortDiscoveryService = serialPortDiscoveryService;
            Reset();
        }

        public void Reset()
        {
            _startedAt = _dateTimeService.UtcNow;
        }
    }
}
