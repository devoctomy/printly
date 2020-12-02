using Microsoft.AspNetCore.Http;
using Printly.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Terminal
{
    public interface IWebSocketTerminalService
    {
        Task RunCommsLoopAsync(
            ISerialPortCommunicationService serialPortCommunicationService,
            CancellationToken cancellationToken);
    }
}
