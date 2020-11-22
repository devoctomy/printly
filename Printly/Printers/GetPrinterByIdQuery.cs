using MediatR;

namespace Printly.Printers
{
    public class GetPrinterByIdQuery : IRequest<GetPrinterByIdQueryResponse>
    {
        public string Id { get; set; }
    }
}
