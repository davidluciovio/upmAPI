using Entity.Dtos.DataProduction.DataProductionLine;
using Entity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataProductionLineController : ControllerBase
    {
        private readonly IServiceCrud<ProductionLineDto, ProductionLineCreateDto, ProductionLineUpdateDto> _productionLineService;

        public DataProductionLineController(IServiceCrud<ProductionLineDto, ProductionLineCreateDto, ProductionLineUpdateDto> productionLineService)
        {
            _productionLineService = productionLineService;
        }

        [HttpGet("v1/get-all")]
        public async Task<IActionResult> GetAllProductionLines()
        {
            try
            {
                var productionLines = await _productionLineService.GetAlls();
                return Ok(productionLines);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet("v1/get-id/{id}")]
        public async Task<IActionResult> GetProductionLineById(Guid id)
        {
            try
            {
                var productionLine = await _productionLineService.GetById(id);
                if (productionLine == null)
                {
                    return NotFound();
                }
                return Ok(productionLine);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/create")]
        public async Task<IActionResult> CreateProductionLine([FromBody] ProductionLineCreateDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return BadRequest("El objeto enviado es nulo.");
                }

                var newProductionLine = await _productionLineService.Create(createDto);

                return CreatedAtAction(nameof(GetProductionLineById), new { id = newProductionLine.Id }, newProductionLine);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/update/{id}")]
        public async Task<IActionResult> UpdateProductionLine(Guid id, [FromBody] ProductionLineUpdateDto updateDto)
        {
            try
            {
                var updatedProductionLine = await _productionLineService.Update(id, updateDto);

                if (updatedProductionLine == null) throw new Exception($"Error al actualizar el ProductionLine con id: {id}");

                return Ok(updatedProductionLine);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }
    }
}
