using MongoDB.Bson;
using Printly.Dto.Response;
using System;
using System.Net;

namespace Printly.Services
{
    public class PrinterIdValidator : IIdValidator
    {
        public Error Validate(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                return new Error
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "Missing id."
                };
            }
            else
            {
                try
                {
                    var objectId = ObjectId.Parse(id);
                }
                catch (Exception ex)
                {
                    return new Error
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        Message = $"Id format incorrect. {ex.Message}"
                    };
                }
            }

            return null;
        }
    }
}
