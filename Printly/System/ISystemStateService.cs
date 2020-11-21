using System;

namespace Printly.System
{
    public interface ISystemStateService
    {
        DateTime StartedAt { get; }
        TimeSpan Uptime { get; }
        string[] SerialPorts { get; }

        void Reset();
    }
}
