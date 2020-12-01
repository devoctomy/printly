using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Printly.Dto.Response;
using MediatR;
using System.Threading;
using System;

namespace Printly.System
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SystemController> _logger;

        public SystemController(
            IMediator mediator,
            ILogger<SystemController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("Info")]
        public async Task<ObjectResponse<SystemInfo>> GetSystemInfo()
        {
            _logger.LogInformation("GetSystemInfo...");
            var response = await _mediator.Send(
                new GetSystemInfoCommand(),
                CancellationToken.None);
            return new ObjectResponse<SystemInfo>
            {
                Success = true,
                Value = response.SystemInfo
            };
        }

    }
}
