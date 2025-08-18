using Microsoft.AspNetCore.Mvc;
using upmDomain.Lider;
using upmDomain.ProductionReport;

namespace upmApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductionReportController : Controller
    {
        private readonly ProductionReportService _productionReportService;
        public ProductionReportController(ProductionReportService productionReportService) 
        {
            _productionReportService = productionReportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductionReportsAsync([FromQuery] DateTime startDatetime, [FromQuery] DateTime endDatetime, [FromQuery] Guid lineId, [FromQuery] int modelId)
        {
            try
            {
                var response = await _productionReportService.GetProductionReportsAsync(startDatetime.ToLocalTime(), endDatetime.ToLocalTime(), lineId, modelId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno en el servidor: {ex.Message}, {ex.InnerException?.Message}");
            }
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
