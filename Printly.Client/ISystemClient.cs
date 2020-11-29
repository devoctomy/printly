using Printly.Dto.Response;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Client
{
    public interface ISystemClient
    {
        Task<ObjectResponse<SystemInfo>> GetInfoAsync(CancellationToken cancellationToken);
    }
}
