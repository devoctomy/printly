﻿using Printly.Dto.Response;

namespace Printly.Printers
{
    public class GetPrinterByIdQueryResponse
    {
        public Printer Printer { get; set; }
        public Error Error { get; set; }
    }
}
