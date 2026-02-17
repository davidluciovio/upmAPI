using Entity.Dtos.ModelDtos.ProductionControl.ProductionLocation;
using Entity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartNumberLocationController : ControllerBase
    {
        private readonly IServiceCrud<PartNumberLocationDto, PartNumberLocationCreateDto, PartNumberLocationUpdateDto> _partNumberLocationService;

        public PartNumberLocationController(IServiceCrud<PartNumberLocationDto, PartNumberLocationCreateDto, PartNumberLocationUpdateDto> partNumberLocationService)
        {
            _partNumberLocationService = partNumberLocationService;
        }

        [HttpGet("v1/get-all")]
        public async Task<IActionResult> GetAllPartNumberLocations()
        {
            try
            {
                var partNumberLocations = await _partNumberLocationService.GetAlls();
                return Ok(partNumberLocations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet("v1/get-id/{id}")]
        public async Task<IActionResult> GetPartNumberLocationById(Guid id)
        {
            try
            {
                var partNumberLocation = await _partNumberLocationService.GetById(id);
                if (partNumberLocation == null)
                {
                    return NotFound();
                }
                return Ok(partNumberLocation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/create")]
        public async Task<IActionResult> CreatePartNumberLocation([FromBody] PartNumberLocationCreateDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return BadRequest("El objeto enviado es nulo.");
                }

                var newPartNumberLocation = await _partNumberLocationService.Create(createDto);

                return CreatedAtAction(nameof(GetPartNumberLocationById), new { id = newPartNumberLocation.Id }, newPartNumberLocation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/update/{id}")]
        public async Task<IActionResult> UpdatePartNumberLocation(Guid id, [FromBody] PartNumberLocationUpdateDto updateDto)
        {
            try
            {
                var updatedPartNumberLocation = await _partNumberLocationService.Update(id, updateDto);

                if (updatedPartNumberLocation == null) throw new Exception($"Error al actualizar el PartNumberLocation con id: {id}");

                return Ok(updatedPartNumberLocation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }
    }
}
