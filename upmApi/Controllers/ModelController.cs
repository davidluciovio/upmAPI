using Microsoft.AspNetCore.Mvc;
using upmDomain.DomainModel;
using upmDomain.Lider;

namespace upmApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModelController : Controller
    {
        private readonly ModelService _modelService;
        public ModelController(ModelService modelService) 
        {
            _modelService = modelService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] List<Guid> lineIds, [FromQuery] List<Guid> liderIds)
        {
            try
            {
                var response = await _modelService.GetAllAsync(lineIds, liderIds);
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
