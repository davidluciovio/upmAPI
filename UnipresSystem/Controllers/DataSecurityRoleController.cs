using Entity.Dtos._00_DataUPM;
using Entity.Dtos._00_DataUPM.DataSecurityRole;
using Entity.Interfaces;
using LogicDomain._00_DataUPM;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataSecurityRoleController : ControllerBase
    {
        private readonly IService<DataSecurityRoleResponseDto, DataSecurityRoleRequestDto> _dataSecurityRoleService;

        public DataSecurityRoleController(IService<DataSecurityRoleResponseDto, DataSecurityRoleRequestDto> dataSecurityRoleService)
        {
            _dataSecurityRoleService = dataSecurityRoleService;
        }

        [HttpGet("v1/get-all")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var Roles = await _dataSecurityRoleService.GetAlls();
                return Ok(Roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet("v1/get-id/{id}")]
        public async Task<IActionResult> GetRoleById(Guid id)
        {
            try
            {
                var Role = await _dataSecurityRoleService.GetById(id);
                if (Role == null)
                {
                    return NotFound();
                }
                return Ok(Role);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/create")]
        public async Task<IActionResult> CreateRole([FromBody] DataSecurityRoleRequestDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return BadRequest("The submitted object is null.");
                }

                var newRole = await _dataSecurityRoleService.Create(createDto);

                return CreatedAtAction(nameof(GetRoleById), new { id = newRole.Id }, newRole);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/update/{id}")]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody] DataSecurityRoleRequestDto updateDto)
        {
            try
            {
                var updatedRole = await _dataSecurityRoleService.Update(id, updateDto);

                if (updatedRole == null)
                {
                    return NotFound($"Role with id: {id} not found.");
                }

                return Ok(updatedRole);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }
    }
}
