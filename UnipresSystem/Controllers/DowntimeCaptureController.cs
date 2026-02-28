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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetDowntime([FromBody] DowntimeRegisterRequestDto dto)
        {
            // 1. Validación básica de nulidad
            if (dto == null)
            {
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
            }

            // 2. Validación automática de DataAnnotations (si usas [Required], [Range], etc. en el DTO)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _downtimeCaptureService.RegisterDowntime(dto);

                // 3. Respuesta semántica: 201 Created es mejor que 200 Ok para creaciones
                return StatusCode(StatusCodes.Status201Created, new { message = "Downtime registered successfully." });
            }
            catch (ArgumentException ex)
            {
                // Errores de lógica de negocio (ej. fechas inválidas)
                return BadRequest(new { error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                // Error cuando no existe el ID de la estación o downtime
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // 4. Loggear el error aquí (p. ej. _logger.LogError(ex, "Error..."))
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { error = "Ocurrió un error interno al procesar el registro." });
            }
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
