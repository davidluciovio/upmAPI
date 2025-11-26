using Entity.Dtos.DataProduction;
using LogicDomain.DataProduction;
using Microsoft.AspNetCore.Mvc;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataProductionPartNumberController : Controller
    {
        private readonly DataProductionPartNumberService _dataProductionPartNumberService;

        public DataProductionPartNumberController(DataProductionPartNumberService DataProductionPartNumberService)
        {
            _dataProductionPartNumberService = DataProductionPartNumberService;
        }

        [HttpPost("v1/create")]
        public async Task<IActionResult> CreateProductionPartNumber([FromBody] DataProductionPartNumberCreateDto PartNumberCreateDto)
        {
            try
            {
                if (PartNumberCreateDto == null)
                {
                    return BadRequest("El objeto enviado es nulo.");
                }

                var newPartNumber = await _dataProductionPartNumberService.CreateProductionPartNumber(PartNumberCreateDto);

                return CreatedAtAction(nameof(GetProductionPartNumberById), new { id = newPartNumber.Id }, newPartNumber);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet("v1/get-all")]
        public async Task<IActionResult> GetAllProductionPartNumbers()
        {
            try
            {
                var PartNumbers = await _dataProductionPartNumberService.GetAllProductionPartNumbers();
                return Ok(PartNumbers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet("v1/get-id/{id}")]
        public async Task<IActionResult> GetProductionPartNumberById(Guid id)
        {
            try
            {
                var PartNumber = await _dataProductionPartNumberService.GetProductionPartNumberById(id);
                if (PartNumber == null)
                {
                    return NotFound();
                }
                return Ok(PartNumber);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/desactivate")]
        public async Task<IActionResult> SetProductionPartNumberDesactivate([FromBody] Guid id)
        {
            try
            {
                var desactivate = await _dataProductionPartNumberService.DeactivateProductionPartNumber(id);
                if (!desactivate) throw new Exception($"Error al desactivar el PartNumber con id: {id}");

                return Ok(desactivate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/activate")]
        public async Task<IActionResult> SetProductionPartNumberActivate([FromBody] Guid id)
        {
            try
            {
                var activate = await _dataProductionPartNumberService.ActivateProductionPartNumber(id);
                if (!activate) throw new Exception($"Error al activar el PartNumber con id: {id}");

                return Ok(activate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/update/{id}")]
        public async Task<IActionResult> SetProductionPartNumberUpdate(Guid id, [FromBody] DataProductionPartNumberDto PartNumber)
        {
            try
            {
                var updatedPartNumber = await _dataProductionPartNumberService.UpdateProductionPartNumber(id, PartNumber);

                if (updatedPartNumber == null) throw new Exception($"Error al actualizar el PartNumber con id: {id}");

                return Ok(updatedPartNumber);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }
    }
}
