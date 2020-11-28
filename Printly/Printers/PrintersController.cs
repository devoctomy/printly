using MediatR;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<GetAllPrintersQueryResponse> Get(
            CancellationToken cancellationToken)
        {
            var request = new GetAllPrintersQuery();
            var response = await _mediator.Send(
                request,
                cancellationToken);
            return response;
        }

        [HttpGet]
        [Route("{id}")]
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

        [HttpPut]
        [Route("{id}")]
        public async Task<UpdatePrinterCommandResponse> Update(
            string id,
            [FromBody] Dto.Request.Printer printer,
            CancellationToken cancellationToken)
        {
            var request = new UpdatePrinterCommand()
            {
                Id = id,
                Printer = printer
            };
            var response = await _mediator.Send(
                request,
                cancellationToken);
            return response;
        }
    }
}
