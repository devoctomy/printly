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
            return Task.FromResult(new GetSystemInfoResponse
            {
                SystemInfo = new SystemInfo
                {
                    SystemId = _systemStateService.Configuration.SystemId,
                    StartedAt = _systemStateService.StartedAt,
                    SerialPorts = _systemStateService.SerialPorts
                }
            });
        }
    }
}
