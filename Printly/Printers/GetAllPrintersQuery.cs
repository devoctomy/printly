﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Printly.Printers
{
    public class GetAllPrintersQuery : IRequest<GetAllPrintersQueryResponse>
    {
    }
}
