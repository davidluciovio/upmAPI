using Entity.Dtos._01_Auth.DataSecuritySubmodule;
using Entity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataSecuritySubmoduleController : ControllerBase
    {
        private readonly IService<DataSecuritySubmoduleResponseDto, DataSecuritySubmoduleRequestDto> _dataSecuritySubmoduleService;

        public DataSecuritySubmoduleController(IService<DataSecuritySubmoduleResponseDto, DataSecuritySubmoduleRequestDto> dataSecuritySubmoduleService)
        {
            _dataSecuritySubmoduleService = dataSecuritySubmoduleService;
        }

        [HttpGet("v1/get-all")]
        public async Task<IActionResult> GetAllSubmodules()
        {
            try
            {
                var submodules = await _dataSecuritySubmoduleService.GetAlls();
                return Ok(submodules);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet("v1/get-id/{id}")]
        public async Task<IActionResult> GetSubmoduleById(Guid id)
        {
            try
            {
                var submodule = await _dataSecuritySubmoduleService.GetById(id);
                if (submodule == null)
                {
                    return NotFound();
                }
                return Ok(submodule);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/create")]
        public async Task<IActionResult> CreateSubmodule([FromBody] DataSecuritySubmoduleRequestDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return BadRequest("The submitted object is null.");
                }

                var newSubmodule = await _dataSecuritySubmoduleService.Create(createDto);

                return CreatedAtAction(nameof(GetSubmoduleById), new { id = newSubmodule.Id }, newSubmodule);
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
        public async Task<IActionResult> UpdateSubmodule(Guid id, [FromBody] DataSecuritySubmoduleRequestDto updateDto)
        {
            try
            {
                var updatedSubmodule = await _dataSecuritySubmoduleService.Update(id, updateDto);
                return Ok(updatedSubmodule);
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