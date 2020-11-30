using System;

namespace Printly.Services
{
    public class SerialPortCommunicationServiceFactory : ISerialPortCommunicationServiceFactory
    {
        public ISerialPortCommunicationService Create()
        {
            return new SerialPortCommunicationService();
        }
    }
}
