using System.Net;

namespace Printly.Dto.Response
{
    public class Response
    {
        public bool Success { get; set; }
        public Error Error { get; set; }

        public Response()
        {
        }

        public Response(
            HttpStatusCode code,
            string message)
        {
            Error = new Error()
            {
                HttpStatusCode = code,
                Message = message
            };
        }
    }
}
