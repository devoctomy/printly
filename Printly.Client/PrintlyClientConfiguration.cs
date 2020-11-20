using System;

namespace Printly.Client
{
    public class PrintlyClientConfiguration
    {
        public string BaseUrl { get; set; }
        public TimeSpan HttpMessageHandlerLifetime { get; set; }
        public int RetryCount { get; set; }
        public TimeSpan SleepDuration { get; set; }
    }
}
