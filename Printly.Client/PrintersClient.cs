using Newtonsoft.Json;
using Printly.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<ObjectResponse<Dto.Response.Printer>> Createsync(
            Dto.Request.Printer printer,
            CancellationToken cancellationToken)
        {
            var response = await _httpAdapter.PostAsync(
                new Uri($"/api/Printers", UriKind.Relative),
                new StringContent(JsonConvert.SerializeObject(printer), Encoding.UTF8, "application/json"),
                cancellationToken);
            return JsonConvert.DeserializeObject<ObjectResponse<Dto.Response.Printer>>(await response.Content.ReadAsStringAsync(cancellationToken));
        }

        public async Task<ObjectResponse<List<Dto.Response.Printer>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var response = await _httpAdapter.GetAsync(
                new Uri("/api/Printers", UriKind.Relative),
                cancellationToken);
            return JsonConvert.DeserializeObject<ObjectResponse<List<Dto.Response.Printer>>>(await response.Content.ReadAsStringAsync(cancellationToken));
        }

        public async Task<ObjectResponse<Dto.Response.Printer>> GetAsync(
            string id,
            CancellationToken cancellationToken)
        {
            var response = await _httpAdapter.GetAsync(
                new Uri($"/api/Printers/{id}", UriKind.Relative),
                cancellationToken);
            return JsonConvert.DeserializeObject<ObjectResponse<Dto.Response.Printer>>(await response.Content.ReadAsStringAsync(cancellationToken));
        }

        public async Task<ObjectResponse<Dto.Response.Printer>> UpdateAsync(
            string id,
            Dto.Request.Printer printer,
            CancellationToken cancellationToken)
        {
            var response = await _httpAdapter.DeleteAsync(
                new Uri($"/api/Printers/{id}", UriKind.Relative),
                cancellationToken);
            return JsonConvert.DeserializeObject<ObjectResponse<Dto.Response.Printer>>(await response.Content.ReadAsStringAsync(cancellationToken));
        }

        public async Task<ObjectResponse<Dto.Response.Printer>> DeleteAsync(
            string id,
            CancellationToken cancellationToken)
        {
            var response = await _httpAdapter.DeleteAsync(
                new Uri($"/api/Printers/{id}", UriKind.Relative),
                cancellationToken);
            return JsonConvert.DeserializeObject<ObjectResponse<Dto.Response.Printer>>(await response.Content.ReadAsStringAsync(cancellationToken));
        }
    }
}
