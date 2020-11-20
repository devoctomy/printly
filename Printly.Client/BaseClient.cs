using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Printly.Client
{
    public class BaseClient
    {
        protected async Task<PrintlyResponse<T>> ProcessHttpResponseMessage<T>(HttpResponseMessage httpResponseMessage)
        {
            var response = new PrintlyResponse<T>()
            {
                Status = httpResponseMessage.StatusCode,
                Value = httpResponseMessage.IsSuccessStatusCode ? JsonConvert.DeserializeObject<T>(await httpResponseMessage.Content.ReadAsStringAsync()) : default(T)
            };

            return response;
        }
    }
}
