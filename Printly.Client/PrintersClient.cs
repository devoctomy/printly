using Newtonsoft.Json;
using Printly.Dto.Response;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Client
{
    public class PrintersClient : BaseClient, IPrintersClient
    {
        private readonly IHttpAdapter<PrintersClient> _httpAdapter;

        public PrintersClient(IHttpAdapter<PrintersClient> httpAdapter)
        {
            _httpAdapter = httpAdapter;
        }

        public async Task<ObjectResponse<Printer>> CreateAsync(
            Dto.Request.Printer printer,
            CancellationToken cancellationToken)
        {
            var response = await _httpAdapter.PostAsync(
                new Uri($"/api/Printers", UriKind.Relative),
                new StringContent(JsonConvert.SerializeObject(printer), Encoding.UTF8, "application/json"),
                cancellationToken);
            return JsonConvert.DeserializeObject<ObjectResponse<Printer>>(await response.Content.ReadAsStringAsync(cancellationToken));
        }

        public async Task<ObjectResponse<List<Printer>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var response = await _httpAdapter.GetAsync(
                new Uri("/api/Printers", UriKind.Relative),
                cancellationToken);
            return JsonConvert.DeserializeObject<ObjectResponse<List<Printer>>>(await response.Content.ReadAsStringAsync(cancellationToken));
        }

        public async Task<ObjectResponse<Printer>> GetAsync(
            string id,
            CancellationToken cancellationToken)
        {
            var response = await _httpAdapter.GetAsync(
                new Uri($"/api/Printers/{id}", UriKind.Relative),
                cancellationToken);
            return JsonConvert.DeserializeObject<ObjectResponse<Printer>>(await response.Content.ReadAsStringAsync(cancellationToken));
        }

        public async Task<ObjectResponse<Printer>> UpdateAsync(
            string id,
            Dto.Request.Printer printer,
            CancellationToken cancellationToken)
        {
            var response = await _httpAdapter.PutAsync(
                new Uri($"/api/Printers/{id}", UriKind.Relative),
                new StringContent(JsonConvert.SerializeObject(printer), Encoding.UTF8, "application/json"),
                cancellationToken);
            return JsonConvert.DeserializeObject<ObjectResponse<Printer>>(await response.Content.ReadAsStringAsync(cancellationToken));
        }

        public async Task<Response> DeleteAsync(
            string id,
            CancellationToken cancellationToken)
        {
            var response = await _httpAdapter.DeleteAsync(
                new Uri($"/api/Printers/{id}", UriKind.Relative),
                cancellationToken);
            return JsonConvert.DeserializeObject<Response>(await response.Content.ReadAsStringAsync(cancellationToken));
        }
    }
}
