namespace Printly.Services
{
    public class SerialPortCommunicationServiceFactory : ISerialPortCommunicationServiceFactory
    {
        private readonly ISerialPortFactory _serialPortFactory;

        public SerialPortCommunicationServiceFactory(ISerialPortFactory serialPortFactory)
        {
            _serialPortFactory = serialPortFactory;
        }

        public ISerialPortCommunicationService Create()
        {
            return new SerialPortCommunicationService(_serialPortFactory);
        }
    }
}
