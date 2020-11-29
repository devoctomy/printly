using AutoMapper;
using MediatR;
using Printly.Domain.Models;
using Printly.Domain.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Printers
{
    public class CreatePrinterCommandHandler : IRequestHandler<CreatePrinterCommand, CreatePrinterCommandResponse>
    {
        private readonly IDataStorageService<Printer> _storageService;
        private readonly IMapper _mapper;

        public CreatePrinterCommandHandler(
            IDataStorageService<Printer> storageService,
            IMapper mapper)
        {
            _storageService = storageService;
            _mapper = mapper;
        }

        public async Task<CreatePrinterCommandResponse> Handle(
            CreatePrinterCommand request,
            CancellationToken cancellationToken)
        {
            var printerDomain = _mapper.Map<Printer>(request.Printer);
            await _storageService.Create(
                printerDomain,
                cancellationToken);
            var printerDto = _mapper.Map<Dto.Response.Printer>(printerDomain);
            return new CreatePrinterCommandResponse()
            {
                Printer = printerDto
            };
        }
    }
}
