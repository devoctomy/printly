using AutoMapper;
using MediatR;
using Printly.Domain.Models;
using Printly.Domain.Services;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Printers
{
    public class GetPrinterByIdQueryHandler : IRequestHandler<GetPrinterByIdQuery, GetPrinterByIdQueryResponse>
    {
        private readonly IDataStorageService<Printer> _storageService;
        private readonly IMapper _mapper;

        public GetPrinterByIdQueryHandler(
            IDataStorageService<Printer> storageService,
            IMapper mapper)
        {
            _storageService = storageService;
            _mapper = mapper;
        }

        public async Task<GetPrinterByIdQueryResponse> Handle(
            GetPrinterByIdQuery request,
            CancellationToken cancellationToken)
        {
            var printer = await _storageService.Get(
                request.Id,
                cancellationToken);
            if(printer != null)
            {
                return new GetPrinterByIdQueryResponse()
                {
                    Printer = _mapper.Map<Printly.Dto.Response.Printer>(printer)
                };
            }
            else
            {
                return new GetPrinterByIdQueryResponse()
                {
                    Error = new Dto.Response.Error()
                    {
                        HttpStatusCode = HttpStatusCode.NotFound,
                        Message = $"Printer with id '{request.Id}' not found."
                    }
                };
            }
        }
    }
}
