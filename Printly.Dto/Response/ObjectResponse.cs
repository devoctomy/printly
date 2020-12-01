using System.Net;

namespace Printly.Dto.Response
{
    public class ObjectResponse<T>
    {
        public bool Success { get; set; }
        public T Value { get; set; }
        public Error Error { get; set; }

        public ObjectResponse()
        {
        }

        public ObjectResponse(
            HttpStatusCode code,
            string message)
        {
            Error = new Error
            {
                HttpStatusCode = code,
                Message = message
            };
        }
    }
}
