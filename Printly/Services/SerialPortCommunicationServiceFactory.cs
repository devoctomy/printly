using Microsoft.Extensions.Logging;

namespace Printly.Services
{
    public class SerialPortCommunicationServiceFactory : ISerialPortCommunicationServiceFactory
    {
        private readonly ISerialPortFactory _serialPortFactory;
        private readonly ILogger<SerialPortCommunicationService> _logger;

        public SerialPortCommunicationServiceFactory(
            ISerialPortFactory serialPortFactory,
            ILogger<SerialPortCommunicationService> logger)
        {
            _serialPortFactory = serialPortFactory;
            _logger = logger;
        }

        public ISerialPortCommunicationService Create()
        {
            return new SerialPortCommunicationService(
                _serialPortFactory,
                _logger);
        }
    }
}
