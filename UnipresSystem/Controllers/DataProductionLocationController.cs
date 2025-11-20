using Entity.Dtos.DataProduction;
using LogicDomain.DataProduction;
using Microsoft.AspNetCore.Mvc;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataProductionLocationController : Controller
    {
        private readonly DataProductionLocationService _dataProductionLocationService;

        public DataProductionLocationController(DataProductionLocationService dataProductionLocationService)
        {
            _dataProductionLocationService = dataProductionLocationService;
        }

        [HttpPost("v1/create")]
        public async Task<IActionResult> CreateProductionLocation([FromBody] DataProductionLocationCreateDto LocationCreateDto)
        {
            try
            {
                if (LocationCreateDto == null)
                {
                    return BadRequest("El objeto enviado es nulo.");
                }

                var newLocation = await _dataProductionLocationService.CreateProductionLocation(LocationCreateDto);

                return CreatedAtAction(nameof(GetProductionLocationById), new { id = newLocation.Id }, newLocation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet("v1/get-all")]
        public async Task<IActionResult> GetAllProductionLocations()
        {
            try
            {
                var Locations = await _dataProductionLocationService.GetAllProductionLocations();
                return Ok(Locations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet("v1/get-id/{id}")]
        public async Task<IActionResult> GetProductionLocationById(Guid id)
        {
            try
            {
                var Location = await _dataProductionLocationService.GetProductionLocationById(id);
                if (Location == null)
                {
                    return NotFound();
                }
                return Ok(Location);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/desactivate")]
        public async Task<IActionResult> SetProductionLocationDesactivate([FromBody] Guid id)
        {
            try
            {
                var desactivate = await _dataProductionLocationService.DeactivateProductionLocation(id);
                if (!desactivate) throw new Exception($"Error al desactivar el Location con id: {id}");

                return Ok(desactivate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/activate")]
        public async Task<IActionResult> SetProductionLocationActivate([FromBody] Guid id)
        {
            try
            {
                var activate = await _dataProductionLocationService.ActivateProductionLocation(id);
                if (!activate) throw new Exception($"Error al activar el Location con id: {id}");

                return Ok(activate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/update/{id}")]
        public async Task<IActionResult> SetProductionLocationUpdate(Guid id, [FromBody] DataProductionLocationDto Location)
        {
            try
            {
                var updatedLocation = await _dataProductionLocationService.UpdateProductionLocation(id, Location);

                if (updatedLocation == null) throw new Exception($"Error al actualizar el Location con id: {id}");

                return Ok(updatedLocation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
