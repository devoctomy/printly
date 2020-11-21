using System;

namespace Printly.Dto.Response
{
    public class SystemInfo
    {
        public DateTime StartedAt { get; set; }
        public TimeSpan Uptime { get; set; }
        public string[] SerialPorts { get; set; }
    }
}
