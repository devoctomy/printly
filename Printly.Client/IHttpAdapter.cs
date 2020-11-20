using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Client
{
    public interface IHttpAdapter<TClient>
    {
        Task<HttpResponseMessage> GetAsync(
            Uri uri,
            CancellationToken cancellationToken);
        Task<HttpResponseMessage> DeleteAsync(
            Uri uri,
            CancellationToken cancellationToken);
        Task<HttpResponseMessage> PutAsync(
            Uri uri,
            object value,
            CancellationToken cancellationToken);
        Task<HttpResponseMessage> PostAsync(
            Uri uri,
            object value,
            CancellationToken cancellationToken);
    }
}
