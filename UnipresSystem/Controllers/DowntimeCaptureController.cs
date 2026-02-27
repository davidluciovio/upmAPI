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
            request.StartDatetime = request.StartDatetime.ToLocalTime();
            request.EndDatetime = request.EndDatetime.ToLocalTime();

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

        [HttpPost("v1/get-line-operators-registers")]
        public async Task<IActionResult> GetLineOperatorsRegisters([FromBody] LineOperatorsRegisterRequestDto dto)
        {
            dto.StartDatetime = dto.StartDatetime.ToLocalTime();
            dto.EndDatetime = dto.EndDatetime.ToLocalTime();
            try
            {
                var result = await _downtimeCaptureService.GetLineOperatorsRegisters(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }


        [HttpPost("v1/register-complete-rack")]
        public async Task<IActionResult> SetCompleteRack([FromBody] CompleteRackRegisterDto dto)
        {
            await _downtimeCaptureService.RegisterCompleteRack(dto);
            return Ok("Complete Rack registered successfully.");
        }

        [HttpPost("v1/register-line-operators")]
        public async Task<IActionResult> SetRegisterLineOperators([FromBody] LineOperatorsRegisterRequestDto dto)
        {
            dto.StartDatetime = dto.StartDatetime.ToLocalTime();
            dto.EndDatetime = dto.EndDatetime.ToLocalTime();
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
        public async Task<IActionResult> SetDowntime([FromBody] DowntimeRegisterRequestDto dto)
        {
            await _downtimeCaptureService.RegisterDowntime(dto);
            return Ok("Downtime registered successfully.");
        }


        [HttpPost("v1/put-line-operators")]
        public async Task<IActionResult> PutLineOperatorRegisters([FromQuery] Guid id, [FromBody] LineOperatorsRegisterRequestDto dto)
        {
            dto.StartDatetime = dto.StartDatetime.ToLocalTime();
            dto.EndDatetime = dto.EndDatetime.ToLocalTime();
            try
            {
                await _downtimeCaptureService.PutLineOperatorRegisters(id, dto);
                return Ok("Line Operators updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }

        }
    }
}
