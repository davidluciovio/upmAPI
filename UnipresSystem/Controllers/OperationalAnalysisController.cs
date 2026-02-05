using Entity.Dtos.AplicationDtos.OperationalAnalysis;
using LogicDomain.ApplicationServices;
using Microsoft.AspNetCore.Mvc;

namespace UnipresSystem.Controllers
{
    [Route("api/[controller]")]
    public class OperationalAnalysisController : Controller
    {
        private readonly OperationalAnalysisService _operationalAnalysisService;
        public OperationalAnalysisController(OperationalAnalysisService operationalAnalysisService)
        {
            _operationalAnalysisService = operationalAnalysisService;
        }


        [HttpGet("v1/get-filters-data")]
        public async Task<IActionResult> GetFiltersData()
        {
            var result = await _operationalAnalysisService.GetFiltersData();
            return Ok(result);
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("v1/get-operational-analysis-data")]
        public async Task<IActionResult> GetOperationalAnalysisData([FromBody] OperationalAnalysisRequestDto request)
        {
            var result = await _operationalAnalysisService.GetOperationalAnalysisData(request);
            return Ok(result);
        }

        [HttpGet("v1/get-operational-analysis-sync")]
        public async Task GetStream(CancellationToken cancellationToken)
        {
            Response.Headers.Add("Content-Type", "text/event-stream");
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Connection", "keep-alive");

            await foreach (var line in _operationalAnalysisService.GetOperationalAnalysisStream(cancellationToken))
            {
                // El formato "data: {mensaje}\n\n" es el estándar para Server-Sent Events
                await Response.WriteAsync($"data: {line}\n\n");
                await Response.Body.FlushAsync(); // ¡Vital para que no se quede en el buffer!
            }
        }

        [HttpGet("v1/get-operational-analysis-partNumberData")]
        public async Task<IActionResult> GetOperationalAnalysisPartNumberData([FromQuery] string partNumber, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _operationalAnalysisService.GetDayOperativity(partNumber, startDate, endDate);
            return Ok(result);
        }
    }
}
