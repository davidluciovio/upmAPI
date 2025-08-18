using Microsoft.AspNetCore.Mvc;
using upmDomain.Lider;
using upmDomain.LineDomain;

namespace upmApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LineController : Controller
    {
        private readonly LineService _lineService;

        public LineController(LineService lineService)
        {
            _lineService = lineService;
        }

        [HttpGet("by-liders")]
        public async Task<IActionResult> GetByLidersAsync([FromQuery] List<Guid> liderIds)
        {
            try
            {
                var response = await _lineService.GetByLidersAsync(liderIds);
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
