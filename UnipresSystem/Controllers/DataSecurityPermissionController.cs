using Entity.Dtos._01_Auth.DataSecurityPermission;
using Entity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataSecurityPermissionController : ControllerBase
    {
        private readonly IService<DataSecurityPermissionResponseDto, DataSecurityPermissionRequestDto> _dataSecurityPermissionService;

        public DataSecurityPermissionController(IService<DataSecurityPermissionResponseDto, DataSecurityPermissionRequestDto> dataSecurityPermissionService)
        {
            _dataSecurityPermissionService = dataSecurityPermissionService;
        }

        [HttpGet("v1/get-all")]
        public async Task<IActionResult> GetAllPermissions()
        {
            try
            {
                var permissions = await _dataSecurityPermissionService.GetAlls();
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet("v1/get-id/{id}")]
        public async Task<IActionResult> GetPermissionById(Guid id)
        {
            try
            {
                var permission = await _dataSecurityPermissionService.GetById(id);
                if (permission == null)
                {
                    return NotFound();
                }
                return Ok(permission);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/create")]
        public async Task<IActionResult> CreatePermission([FromBody] DataSecurityPermissionRequestDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return BadRequest("The submitted object is null.");
                }

                var newPermission = await _dataSecurityPermissionService.Create(createDto);

                return CreatedAtAction(nameof(GetPermissionById), new { id = newPermission.Id }, newPermission);
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
        public async Task<IActionResult> UpdatePermission(Guid id, [FromBody] DataSecurityPermissionRequestDto updateDto)
        {
            try
            {
                var updatedPermission = await _dataSecurityPermissionService.Update(id, updateDto);
                return Ok(updatedPermission);
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
