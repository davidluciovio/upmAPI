using Entity.Dtos._00_DataUPM.DataSecurityModule;
using Entity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataSecurityModuleController : ControllerBase
    {
        private readonly IService<DataSecurityModuleResponseDto, DataSecurityModuleRequestDto> _dataSecurityModuleService;

        public DataSecurityModuleController(IService<DataSecurityModuleResponseDto, DataSecurityModuleRequestDto> dataSecurityModuleService)
        {
            _dataSecurityModuleService = dataSecurityModuleService;
        }

        [HttpGet("v1/get-all")]
        public async Task<IActionResult> GetAllModules()
        {
            try
            {
                var modules = await _dataSecurityModuleService.GetAlls();
                return Ok(modules);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet("v1/get-id/{id}")]
        public async Task<IActionResult> GetModuleById(Guid id)
        {
            try
            {
                var module = await _dataSecurityModuleService.GetById(id);
                if (module == null)
                {
                    return NotFound();
                }
                return Ok(module);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/create")]
        public async Task<IActionResult> CreateModule([FromBody] DataSecurityModuleRequestDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return BadRequest("The submitted object is null.");
                }

                var newModule = await _dataSecurityModuleService.Create(createDto);

                return CreatedAtAction(nameof(GetModuleById), new { id = newModule.Id }, newModule);
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

        [HttpPut("v1/update/{id}")]
        public async Task<IActionResult> UpdateModule(Guid id, [FromBody] DataSecurityModuleRequestDto updateDto)
        {
            try
            {
                var updatedModule = await _dataSecurityModuleService.Update(id, updateDto);
                return Ok(updatedModule);
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
    }
}
