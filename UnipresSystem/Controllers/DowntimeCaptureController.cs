using LogicDomain.ApplicationServices;
using Microsoft.AspNetCore.Mvc;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DowntimeCaptureController : Controller
    {
        private readonly DowntimeCaptureService _downtimeCaptureService;

        public DowntimeCaptureController(DowntimeCaptureService downtimeCaptureService)
        {
            _downtimeCaptureService = downtimeCaptureService;
        }

        [HttpPost("v1/get-downtime-capture-data")]
        public async Task<IActionResult> GetDowntimeCaptureData([FromBody] Entity.AplicationDtos.DowntimeCapture.DowntimeCaptureRequestDto request)
        {
            var result = await _downtimeCaptureService.GetDowntimeCaptureData(request);
            return Ok(result);
        }
    }
}
