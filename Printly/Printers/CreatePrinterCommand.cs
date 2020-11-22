using MediatR;
using Printly.Dto.Request;

namespace Printly.Printers
{
    public class CreatePrinterCommand : IRequest<CreatePrinterCommandResponse>
    {
        public Printer Printer { get; set; }
    }
}
