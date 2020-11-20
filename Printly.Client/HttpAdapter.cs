using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Client
{
    public class HttpAdapter<TClient> : IHttpAdapter<TClient>
    {
        private readonly HttpClient _httpClient;

        public HttpAdapter(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<HttpResponseMessage> DeleteAsync(Uri uri, CancellationToken cancellationToken)
        {
            return _httpClient.DeleteAsync(
                uri,
                cancellationToken);
        }

        public Task<HttpResponseMessage> GetAsync(Uri uri, CancellationToken cancellationToken)
        {
            return _httpClient.GetAsync(
                uri,
                cancellationToken);
        }

        public Task<HttpResponseMessage> PostAsync(Uri uri, object value, CancellationToken cancellationToken)
        {
            return _httpClient.PostAsync(
                uri,
                new StringContent(JsonConvert.SerializeObject(value), System.Text.Encoding.UTF8),
                cancellationToken);
        }

        public Task<HttpResponseMessage> PutAsync(Uri uri, object value, CancellationToken cancellationToken)
        {
            return _httpClient.PutAsync(
                uri,
                new StringContent(JsonConvert.SerializeObject(value), System.Text.Encoding.UTF8),
                cancellationToken);
        }
    }
}
