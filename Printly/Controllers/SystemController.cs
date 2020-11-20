using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Printly.Dto.Response;
using Printly.Services;

namespace Printly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly ISystemStateService _systemStateService;
        private readonly ILogger<SystemController> _logger;

        public SystemController(
            ISystemStateService systemStateService,
            ILogger<SystemController> logger)
        {
            _systemStateService = systemStateService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<SystemInfo> GetInfo()
        {
            _logger.LogInformation("GetInfo...");
            await Task.Yield();
            return new SystemInfo()
            {
                StartedAt = _systemStateService.StartedAt,
                Uptime = _systemStateService.Uptime
            };
        }

    }
}
