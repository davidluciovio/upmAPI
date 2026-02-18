using Entity.Dtos.ModelDtos.ProductionControl.ComponentAlert;
using Entity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComponentAlertController : ControllerBase
    {
        private readonly IServiceCrud<ComponentAlertResponseDto, ComponentAlertRequestDto, ComponentAlertRequestDto> _componentAlertService;

        public ComponentAlertController(IServiceCrud<ComponentAlertResponseDto, ComponentAlertRequestDto, ComponentAlertRequestDto> componentAlertService)
        {
            _componentAlertService = componentAlertService;
        }

        [HttpGet("v1/get-all")]
        public async Task<IActionResult> GetAllComponentAlerts()
        {
            try
            {
                var componentAlerts = await _componentAlertService.GetAlls();
                return Ok(componentAlerts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet("v1/get-id/{id}")]
        public async Task<IActionResult> GetComponentAlertById(Guid id)
        {
            try
            {
                var componentAlert = await _componentAlertService.GetById(id);
                if (componentAlert == null)
                {
                    return NotFound();
                }
                return Ok(componentAlert);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/create")]
        public async Task<IActionResult> CreateComponentAlert([FromBody] ComponentAlertRequestDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return BadRequest("El objeto enviado es nulo.");
                }

                var newComponentAlert = await _componentAlertService.Create(createDto);

                return CreatedAtAction(nameof(GetComponentAlertById), new { id = newComponentAlert.Id }, newComponentAlert);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/update/{id}")]
        public async Task<IActionResult> UpdateComponentAlert(Guid id, [FromBody] ComponentAlertRequestDto updateDto)
        {
            try
            {
                var updatedComponentAlert = await _componentAlertService.Update(id, updateDto);

                if (updatedComponentAlert == null) throw new Exception($"Error al actualizar la alerta de componente con id: {id}");

                return Ok(updatedComponentAlert);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }
    }
}
