using System;

namespace Printly.Services
{
    public class DateTimeService : IDateTimeService
    {
        DateTime IDateTimeService.UtcNow => DateTime.UtcNow;
    }
}
