using Entity.Dtos.ModelDtos.ProductionControl.ComponentAlert;
using Entity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductionControlComponentAlertController : ControllerBase
    {
        private readonly IComponentAlertService _service;

        public ProductionControlComponentAlertController(IComponentAlertService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComponentAlertDto>>> GetAll()
        {
            var items = await _service.GetAlls();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComponentAlertDto>> GetById(Guid id)
        {
            var item = await _service.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ComponentAlertDto>> Create(ComponentAlertCreateDto createDto)
        {
            var newItem = await _service.Create(createDto);
            return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ComponentAlertUpdateDto updateDto)
        {
            try
            {
                var updatedItem = await _service.Update(id, updateDto);
                return Ok(updatedItem);
            }
            catch (Exception ex)
            {
                // TODO: Log exception
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.Delete(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
