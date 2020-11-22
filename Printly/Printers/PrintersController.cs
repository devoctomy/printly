using MediatR;
using Microsoft.AspNetCore.Mvc;
using Printly.Dto.Response;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Printers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrintersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PrintersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<GetPrinterByIdQueryResponse> Get(
            string id,
            CancellationToken cancellationToken)
        {
            var request = new GetPrinterByIdQuery()
            {
                Id = id
            };
            var response = await _mediator.Send(
                request,
                cancellationToken);
            return response;
        }

        [HttpPost]
        public async Task<CreatePrinterCommandResponse> Create(
            [FromBody] Dto.Request.Printer printer,
            CancellationToken cancellationToken)
        {
            var request = new CreatePrinterCommand()
            {
                Printer = printer
            };
            var response = await _mediator.Send(
                request,
                cancellationToken);
            return response;
        }
    }
}
