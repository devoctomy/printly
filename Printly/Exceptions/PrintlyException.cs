using System;

namespace Printly.Exceptions
{
    public class PrintlyException : Exception
    {
        public PrintlyException(string message)
            :base(message)
        {
        }

        public PrintlyException(
            string message,
            Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
