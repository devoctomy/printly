using MediatR;
using Printly.Dto.Response;
using System.Threading;
using System.Threading.Tasks;

namespace Printly.System
{
    public class GetSystemInfoCommandHandler : IRequestHandler<GetSystemInfoCommand, GetSystemInfoResponse>
    {
        private readonly ISystemStateService _systemStateService;

        public GetSystemInfoCommandHandler(ISystemStateService systemStateService)
        {
            _systemStateService = systemStateService;
        }

        public Task<GetSystemInfoResponse> Handle(
            GetSystemInfoCommand request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new GetSystemInfoResponse()
            {
                SystemInfo = new SystemInfo()
                {
                    StartedAt = _systemStateService.StartedAt,
                    Uptime = _systemStateService.Uptime,
                    SerialPorts = _systemStateService.SerialPorts
                }
            });
        }
    }
}
