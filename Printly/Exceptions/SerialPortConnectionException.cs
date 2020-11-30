namespace Printly.Exceptions
{
    public class SerialPortConnectionException : PrintlyException
    {
        public SerialPortConnectionException(string message)
            :base(message)
        {
        }
    }
}
