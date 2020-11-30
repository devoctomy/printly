using Newtonsoft.Json;
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

        public async Task<ObjectResponse<SystemInfo>> GetInfoAsync(CancellationToken cancellationToken)
        {
            var response = await _httpAdapter.GetAsync(
                new Uri("/api/System/Info", UriKind.Relative),
                cancellationToken);
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonConvert.DeserializeObject<ObjectResponse<SystemInfo>> (await response.Content.ReadAsStringAsync(cancellationToken));
        }
    }
}
