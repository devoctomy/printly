using System;

namespace Printly.Services
{
    public class SystemStateService : ISystemStateService
    {
        private readonly DateTime _startedAt = DateTime.UtcNow;

        public DateTime StartedAt => _startedAt;
        public TimeSpan Uptime => DateTime.UtcNow - StartedAt;
    }
}
