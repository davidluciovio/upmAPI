using Entity.Dtos.ProductionControl;
using Entity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartNumberAreaController : ControllerBase
    {
        private readonly IServiceCrud<PartNumberLogisticsResponseDto, PartNumberLogisticsCreateDto, PartNumberLogisticsUpdateDto> _partNumberAreaService;

        public PartNumberAreaController(IServiceCrud<PartNumberLogisticsResponseDto, PartNumberLogisticsCreateDto, PartNumberLogisticsUpdateDto> partNumberAreaService)
        {
            _partNumberAreaService = partNumberAreaService;
        }

        [HttpGet("v1/get-all")]
        public async Task<IActionResult> GetAllPartNumberAreas()
        {
            try
            {
                var partNumberAreas = await _partNumberAreaService.GetAlls();
                return Ok(partNumberAreas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet("v1/get-id/{id}")]
        public async Task<IActionResult> GetPartNumberAreaById(Guid id)
        {
            try
            {
                var partNumberArea = await _partNumberAreaService.GetById(id);
                if (partNumberArea == null)
                {
                    return NotFound();
                }
                return Ok(partNumberArea);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/create")]
        public async Task<IActionResult> CreatePartNumberArea([FromBody] PartNumberLogisticsCreateDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return BadRequest("El objeto enviado es nulo.");
                }

                var newPartNumberArea = await _partNumberAreaService.Create(createDto);

                return CreatedAtAction(nameof(GetPartNumberAreaById), new { id = newPartNumberArea.Id }, newPartNumberArea);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/update/{id}")]
        public async Task<IActionResult> UpdatePartNumberArea(Guid id, [FromBody] PartNumberLogisticsUpdateDto updateDto)
        {
            try
            {
                var updatedPartNumberArea = await _partNumberAreaService.Update(id, updateDto);

                if (updatedPartNumberArea == null) throw new Exception($"Error al actualizar el PartNumberArea con id: {id}");

                return Ok(updatedPartNumberArea);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }
    }
}
