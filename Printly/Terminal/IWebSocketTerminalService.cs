using Printly.Services;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Terminal
{
    public interface IWebSocketTerminalService
    {
        WebSocket WebSocket { get; set; }

        Task RunCommsLoopAsync(
            ISerialPortCommunicationService serialPortCommunicationService,
            CancellationToken cancellationToken);
    }
}
