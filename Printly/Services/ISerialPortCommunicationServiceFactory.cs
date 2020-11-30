namespace Printly.Services
{
    public interface ISerialPortCommunicationServiceFactory
    {
        ISerialPortCommunicationService Create();
    }
}
