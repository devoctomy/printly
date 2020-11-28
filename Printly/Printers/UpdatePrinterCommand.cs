using MediatR;
using Printly.Dto.Request;

namespace Printly.Printers
{
    public class UpdatePrinterCommand : IRequest<UpdatePrinterCommandResponse>
    {
        public string Id { get; set; }
        public Printer Printer { get; set; }
    }
}
