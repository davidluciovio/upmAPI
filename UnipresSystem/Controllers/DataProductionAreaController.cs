using Entity.Dtos.DataProduction;
using LogicDomain.DataProduction;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataProductionAreaController : Controller
    {
        private readonly DataProductionAreaService _dataProductionAreaService;

        public DataProductionAreaController(DataProductionAreaService dataProductionAreaService)
        {
            _dataProductionAreaService = dataProductionAreaService;
        }

        [HttpPost("v1/create")]
        public async Task<IActionResult> CreateProductionArea([FromBody] DataProductionAreaCreateDto areaCreateDto)
        {
            try
            {
                if (areaCreateDto == null)
                {
                    return BadRequest("El objeto enviado es nulo.");
                }

                var newArea = await _dataProductionAreaService.CreateProductionArea(areaCreateDto);

                return CreatedAtAction(nameof(GetProductionAreaById), new { id = newArea.Id }, newArea);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet("v1/get-all")]
        public async Task<IActionResult> GetAllProductionAreas()
        {
            try
            {
                var areas = await _dataProductionAreaService.GetAllProductionAreas();
                return Ok(areas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet("v1/get-id/{id}")]
        public async Task<IActionResult> GetProductionAreaById(Guid id)
        {
            try
            {
                var area = await _dataProductionAreaService.GetProductionAreaById(id);
                if (area == null)
                {
                    return NotFound();
                }
                return Ok(area);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/desactivate")]
        public async Task<IActionResult> SetProductionAreaDesactivate([FromBody] Guid id)
        {
            try
            {
                var desactivate = await _dataProductionAreaService.DeactivateProductionArea(id);
                if (!desactivate) throw new Exception($"Error al desactivar el area con id: {id}");

                return Ok(desactivate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/activate")]
        public async Task<IActionResult> SetProductionAreaActivate([FromBody] Guid id)
        {
            try
            {
                var activate = await _dataProductionAreaService.ActivateProductionArea(id);
                if (!activate) throw new Exception($"Error al activar el area con id: {id}");

                return Ok(activate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/update/{id}")]
        public async Task<IActionResult> SetProductionAreaUpdate(Guid id, [FromBody] DataProductionAreaDto area)
        {
            try
            {
                var updatedArea = await _dataProductionAreaService.UpdateProductionArea(id, area);

                if (updatedArea == null) throw new Exception($"Error al actualizar el area con id: {id}");

                return Ok(updatedArea);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }
    }
}