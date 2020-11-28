using MediatR;

namespace Printly.Printers
{
    public class DeletePrinterByIdCommand : IRequest<DeletePrinterByIdCommandResponse>
    {
        public string Id { get; set; }
    }
}
