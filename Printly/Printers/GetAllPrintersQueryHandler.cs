using AutoMapper;
using MediatR;
using Printly.Domain.Models;
using Printly.Domain.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Printers
{
    public class GetAllPrintersQueryHandler : IRequestHandler<GetAllPrintersQuery, GetAllPrintersQueryResponse>
    {
        private readonly IDataStorageService<Printer> _storageService;
        private readonly IMapper _mapper;

        public GetAllPrintersQueryHandler(
            IDataStorageService<Printer> storageService,
            IMapper mapper)
        {
            _storageService = storageService;
            _mapper = mapper;
        }

        public async Task<GetAllPrintersQueryResponse> Handle(
            GetAllPrintersQuery request,
            CancellationToken cancellationToken)
        {
            var printers = await _storageService.Get(cancellationToken);
            return new GetAllPrintersQueryResponse()
            {
                Printers = _mapper.Map<List<Dto.Response.Printer>>(printers)
            };
        }
    }
}
