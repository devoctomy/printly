﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using MongoDB.Bson;
using Printly.Domain.Models;
using Printly.Domain.Services;
using System;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.Printers
{
    public class UpdatePrinterCommandHandler : IRequestHandler<UpdatePrinterCommand, UpdatePrinterCommandResponse>
    {
        private readonly IDataStorageService<Printer> _storageService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer _stringLocalizer;

        public UpdatePrinterCommandHandler(
            IDataStorageService<Printer> storageService,
            IMapper mapper,
            IStringLocalizerFactory stringLocalizerFactory)
        {
            _storageService = storageService;
            _mapper = mapper;
            var assemblyName = new AssemblyName(this.GetType().Assembly.FullName);
            _stringLocalizer = stringLocalizerFactory.Create("Printly", assemblyName.Name);
        }

        public async Task<UpdatePrinterCommandResponse> Handle(
            UpdatePrinterCommand request,
            CancellationToken cancellationToken)
        {
            var printerDomain = _mapper.Map<Printer>(request.Printer);
            printerDomain.Id = ObjectId.Parse(request.Id);
            var result = await _storageService.Update(
                request.Id,
                printerDomain,
                cancellationToken);
            if (result.IsAcknowledged && result.IsModifiedCountAvailable && result.ModifiedCount == 1)
            {
                return new UpdatePrinterCommandResponse();
            }
            else
            {
                return new UpdatePrinterCommandResponse
                {
                    Error = new Dto.Response.Error
                    {
                        HttpStatusCode = HttpStatusCode.NotFound,
                        Message = _stringLocalizer["PrinterNotFound", request.Id]
                    }
                };
            }
        }
    }
}
