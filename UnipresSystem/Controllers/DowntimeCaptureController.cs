using Entity.Dtos.AplicationDtos.DowntimeCapture;
using LogicDomain._00_DataUPM;
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
        public async Task<IActionResult> GetDowntimeCaptureData([FromBody] DowntimeCaptureRequestDto request)
        {
            var result = await _downtimeCaptureService.GetDowntimeCaptureData(request);
            return Ok(result);
        }

        [HttpGet("v1/get-active-employees")]
        public async Task<IActionResult> GetActiveEmployees([FromQuery] string req)
        {
            try
            {
                var result = await _downtimeCaptureService.GetActiveEmployees(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }
        [HttpPost("v1/register-complete-rack")]
        public async Task<IActionResult> RegisterCompleteRack([FromBody] CompleteRackRegisterDto dto)
        {
            await _downtimeCaptureService.RegisterCompleteRack(dto);
            return Ok("Complete Rack registered successfully.");
        }

        [HttpPost("v1/register-line-operators")]
        public async Task<IActionResult> RegisterLineOperators([FromBody] LineOperatorsRegisterRequestDto dto)
        {
            try
            {
                await _downtimeCaptureService.RegisterLineOperators(dto);
                return Ok("Line Operators registered successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/register-downtime")]
        public async Task<IActionResult> RegisterDowntime([FromBody] DowntimeRegisterDto dto)
        {
            await _downtimeCaptureService.RegisterDowntime(dto);
            return Ok("Downtime registered successfully.");
        }
    }
}
