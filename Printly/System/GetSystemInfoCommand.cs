using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Printly.System
{
    public class GetSystemInfoCommand : IRequest<GetSystemInfoResponse>
    {
    }
}
