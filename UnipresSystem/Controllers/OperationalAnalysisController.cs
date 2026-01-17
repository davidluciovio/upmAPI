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
        public async Task<IActionResult> GetOperationalAnalysisData([FromBody] Entity.AplicationDtos.OperationalAnalysis.OperationalAnalysisRequestDto request)
        {
            var result = await _operationalAnalysisService.GetOperationalAnalysisData(request);
            return Ok(result);
        }
    }
}
