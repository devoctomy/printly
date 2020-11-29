using Printly.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.System
{
    public interface ISystemStateService
    {
        Configuration Configuration { get; set; }
        DateTime StartedAt { get; }
        TimeSpan Uptime { get; }
        string[] SerialPorts { get; }

        Task InitialiseAsync(CancellationToken cancellationToken);
        void Reset();
    }
}
