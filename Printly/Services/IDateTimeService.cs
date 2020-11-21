using System;

namespace Printly.Services
{
    public interface IDateTimeService
    {
        DateTime UtcNow { get; }
    }
}
