using AutoMapper;
using MediatR;
using Printly.Domain.Models;
using Printly.Domain.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Printers
{
    public class DeletePrinterByIdCommandHandler : IRequestHandler<DeletePrinterByIdCommand, DeletePrinterByIdCommandResponse>
    {
        private readonly IDataStorageService<Printer> _storageService;

        public DeletePrinterByIdCommandHandler(IDataStorageService<Printer> storageService)
        {
            _storageService = storageService;
        }

        public async Task<DeletePrinterByIdCommandResponse> Handle(
            DeletePrinterByIdCommand request,
            CancellationToken cancellationToken)
        {
            var result = await _storageService.Remove(request.Id);
            return new DeletePrinterByIdCommandResponse()
            {
                IsAcknowledged = result.IsAcknowledged
            };
        }
    }
}
