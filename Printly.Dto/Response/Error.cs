using System.Net;

namespace Printly.Dto.Response
{
    public class Error
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string Message { get; set; }
    }
}
