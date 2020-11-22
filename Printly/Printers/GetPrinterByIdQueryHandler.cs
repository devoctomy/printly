using AutoMapper;
using MediatR;
using Printly.Domain.Models;
using Printly.Domain.Services;
using System;
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
            await Task.Yield();
            var test = new Printer()
            {
                Id = request.Id,
                MarlinId = Guid.NewGuid().ToString(),
                Name = "This is a test!"
            };
            return new GetPrinterByIdQueryResponse()
            {
                Printer = _mapper.Map<Printly.Dto.Response.Printer>(test)
            };
        }
    }
}
