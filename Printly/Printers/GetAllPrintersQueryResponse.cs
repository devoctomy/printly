using Printly.Dto.Response;
using System.Collections.Generic;

namespace Printly.Printers
{
    public class GetAllPrintersQueryResponse
    {
        public List<Printer> Printers { get; set; }
        public Error Error { get; set; }
    }
}
