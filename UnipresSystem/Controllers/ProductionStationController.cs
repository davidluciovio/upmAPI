using Entity.Dtos.AssyProduction;
using Entity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductionStationController : ControllerBase
    {
        private readonly IServiceCrud<ProductionStationResponseDto, ProductionStationCreateDto, ProductionStationUpdateDto> _productionStationService;

        public ProductionStationController(IServiceCrud<ProductionStationResponseDto, ProductionStationCreateDto, ProductionStationUpdateDto> productionStationService)
        {
            _productionStationService = productionStationService;
        }

        [HttpGet("v1/get-all")]
        public async Task<IActionResult> GetAllProductionStations()
        {
            try
            {   
                var productionStations = await _productionStationService.GetAlls();
                return Ok(productionStations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet("v1/get-id/{id}")]
        public async Task<IActionResult> GetProductionStationById(Guid id)
        {
            try
            {
                var productionStation = await _productionStationService.GetById(id);
                if (productionStation == null)
                {
                    return NotFound();
                }
                return Ok(productionStation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/create")]
        public async Task<IActionResult> CreateProductionStation([FromBody] ProductionStationCreateDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return BadRequest("El objeto enviado es nulo.");
                }

                var newProductionStation = await _productionStationService.Create(createDto);

                return CreatedAtAction(nameof(GetProductionStationById), new { id = newProductionStation.Id }, newProductionStation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/update/{id}")]
        public async Task<IActionResult> UpdateProductionStation(Guid id, [FromBody] ProductionStationUpdateDto updateDto)
        {
            try
            {
                var updatedProductionStation = await _productionStationService.Update(id, updateDto);

                if (updatedProductionStation == null) throw new Exception($"Error al actualizar el ProductionStation con id: {id}");

                return Ok(updatedProductionStation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }
    }
}
