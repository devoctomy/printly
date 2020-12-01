using MediatR;
using Microsoft.AspNetCore.Mvc;
using Printly.Dto.Response;
using System.Collections.Generic;
using System.Net;
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
        public async Task<ObjectResponse<List<Printer>>> Get(
            CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid)
            {
                return new ObjectResponse<List<Printer>>(
                    HttpStatusCode.BadRequest,
                    "Model state is invalid.");
            }

            var request = new GetAllPrintersQuery();
            var response = await _mediator.Send(
                request,
                cancellationToken);
            return new ObjectResponse<List<Printer>>
            {
                Success = response.Error == null,
                Value = response.Printers,
                Error = response.Error
            };
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ObjectResponse<Printer>> Get(
            string id,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return new ObjectResponse<Printer>(
                    HttpStatusCode.BadRequest,
                    "Model state is invalid.");
            }

            var request = new GetPrinterByIdQuery
            {
                Id = id
            };
            var response = await _mediator.Send(
                request,
                cancellationToken);
            return new ObjectResponse<Printer>
            {
                Success = response.Error == null,
                Value = response.Printer,
                Error = response.Error
            };
        }

        [HttpPost]
        public async Task<ObjectResponse<Printer>> Create(
            [FromBody] Dto.Request.Printer printer,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return new ObjectResponse<Printer>(
                    HttpStatusCode.BadRequest,
                    "Model state is invalid.");
            }

            var request = new CreatePrinterCommand
            {
                Printer = printer
            };
            var response = await _mediator.Send(
                request,
                cancellationToken);
            return new ObjectResponse<Printer>
            {
                Success = true,
                Value = response.Printer
            };
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<Response> Update(
            string id,
            [FromBody] Dto.Request.Printer printer,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return new Response(
                    HttpStatusCode.BadRequest,
                    "Model state is invalid.");
            }

            var request = new UpdatePrinterCommand
            {
                Id = id,
                Printer = printer
            };
            var response = await _mediator.Send(
                request,
                cancellationToken);
            return new Response
            {
                Success = response.Error == null,
                Error = response.Error
            };
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<Response> Delete(
            string id,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return new Response(
                    HttpStatusCode.BadRequest,
                    "Model state is invalid.");
            }

            var request = new DeletePrinterByIdCommand
            {
                Id = id
            };
            var response = await _mediator.Send(
                request,
                cancellationToken);
            return new Response
            {
                Error = response.Error
            };
        }
    }
}
