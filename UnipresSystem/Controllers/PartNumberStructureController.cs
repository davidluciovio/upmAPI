using Entity.Dtos.ModelDtos.ProductionControl.PartNumberStructure;
using LogicDomain.ModelServices.ProductionControl;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartNumberStructureController : ControllerBase
    {
        private readonly PartNumberStructureService _service;

        public PartNumberStructureController(PartNumberStructureService service)
        {
            _service = service;
        }

        [HttpGet("v1/get-all")]
        public async Task<IActionResult> GetAllPartNumberStructures()
        {
            try
            {
                var items = await _service.GetAlls();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet("v1/get-by-id/{id}")]
        public async Task<IActionResult> GetPartNumberStructureById(Guid id)
        {
            try
            {
                var item = await _service.GetById(id);
                if (item == null)
                {
                    return NotFound();
                }
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/create")]
        public async Task<IActionResult> CreatePartNumberStructure([FromBody] PartNumberStructureRequestDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return BadRequest("The submitted object is null.");
                }
                var newItem = await _service.Create(createDto);
                return CreatedAtAction(nameof(GetPartNumberStructureById), new { id = newItem.Id }, newItem);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/update/{id}")]
        public async Task<IActionResult> UpdatePartNumberStructure(Guid id, [FromBody] PartNumberStructureRequestDto updateDto)
        {
            try
            {
                var updatedItem = await _service.Update(id, updateDto);
                return Ok(updatedItem);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpDelete("v1/delete/{id}")]
        public async Task<IActionResult> DeletePartNumberStructure(Guid id)
        {
            try
            {
                var result = await _service.Delete(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }
    }
}
