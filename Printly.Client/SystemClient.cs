using Printly.Dto.Response;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Client
{
    public class SystemClient : BaseClient, ISystemClient
    {
        private readonly IHttpAdapter<SystemClient> _httpAdapter;

        public SystemClient(IHttpAdapter<SystemClient> httpAdapter)
        {
            _httpAdapter = httpAdapter;
        }

        public async Task<PrintlyResponse<SystemInfo>> GetInfoAsync(CancellationToken cancellationToken)
        {
            var response = await _httpAdapter.GetAsync(
                new Uri("/api/System"),
                cancellationToken);
            return await ProcessHttpResponseMessage<SystemInfo>(response);
        }
    }
}
