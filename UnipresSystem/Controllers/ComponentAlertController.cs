using Entity.Dtos.AplicationDtos.ComponentAlert;
using Entity.Interfaces;
using LogicDomain.ApplicationServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComponentAlertController : ControllerBase
    {
        private readonly ComponentAlertService _componentAlertService;

        public ComponentAlertController(ComponentAlertService componentAlertService)
        {
            _componentAlertService = componentAlertService;
        }

        [HttpGet("v1/get-all")]
        public async Task<IActionResult> GetAllComponentAlerts()
        {
            try
            {
                var componentAlerts = await _componentAlertService.GetAllComponentAlerts();
                return Ok(componentAlerts);
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
                var newComponentAlert = await _componentAlertService.CreateComponentAlert(createDto);
                return CreatedAtAction(nameof(GetComponentAlertById), new { id = newComponentAlert.Id }, newComponentAlert);
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
                var componentAlert = await _componentAlertService.GetComponentAlertById(id);
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

        [HttpPost("v1/update/{id}")]
        public async Task<IActionResult> UpdateComponentAlert(Guid id, [FromBody] ComponentAlertRequestDto updateDto)
        {
            try
            {
                var updatedComponentAlert = await _componentAlertService.UpdateComponentAlert(id, updateDto);

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
