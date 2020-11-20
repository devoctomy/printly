using System.Net;

namespace Printly.Client
{
    public class PrintlyResponse<T>
    {
        public HttpStatusCode Status { get; set; }
        public T Value { get; set; }
    }
}
