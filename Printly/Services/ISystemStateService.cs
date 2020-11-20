using System;

namespace Printly.Services
{
    public interface ISystemStateService
    {
        public DateTime StartedAt { get; }
        public TimeSpan Uptime { get; }
    }
}
