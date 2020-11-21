using Printly.Services;
using System;

namespace Printly.System
{
    public class SystemStateService : ISystemStateService
    {
        private readonly IDateTimeService _dateTimeService;
        private DateTime _startedAt;

        public DateTime StartedAt => _startedAt;
        public TimeSpan Uptime => DateTime.UtcNow - StartedAt;

        public SystemStateService(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
            Reset();
        }

        public void Reset()
        {
            _startedAt = _dateTimeService.UtcNow;
        }
    }
}
