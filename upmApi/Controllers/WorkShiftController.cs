using Microsoft.AspNetCore.Mvc;
using upmDomain.Lider;
using upmDomain.Shift;

namespace upmApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkShiftController : Controller
    {
        private readonly WorkShiftService _workShiftService;

        public WorkShiftController(WorkShiftService workShiftService)
        {
            _workShiftService = workShiftService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var response = await _workShiftService.GetAllAsync();
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
