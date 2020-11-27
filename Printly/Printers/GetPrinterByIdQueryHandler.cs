﻿using AutoMapper;
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
            var printer = await _storageService.Get(request.Id);
            return new GetPrinterByIdQueryResponse()
            {
                Printer = _mapper.Map<Printly.Dto.Response.Printer>(printer)
            };
        }
    }
}