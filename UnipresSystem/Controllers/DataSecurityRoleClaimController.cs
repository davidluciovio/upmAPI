using Entity.Dtos._01_Auth.DataSecurityRoleClaim;
using Entity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataSecurityRoleClaimController : ControllerBase
    {
        private readonly IServiceRoleClaim<DataSecurityRoleClaimResponseDto, DataSecurityRoleClaimRequestDto> _roleClaimService;

        public DataSecurityRoleClaimController(IServiceRoleClaim<DataSecurityRoleClaimResponseDto, DataSecurityRoleClaimRequestDto> roleClaimService)
        {
            _roleClaimService = roleClaimService;
        }

        [HttpGet("v1/get-all")]
        public async Task<IActionResult> GetAllRoleClaims()
        {
            try
            {
                var roleClaims = await _roleClaimService.GetAlls();
                return Ok(roleClaims);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet("v1/get-id/{id}")]
        public async Task<IActionResult> GetRoleClaimById(int id)
        {
            try
            {
                var roleClaim = await _roleClaimService.GetById(id);
                if (roleClaim == null)
                {
                    return NotFound();
                }
                return Ok(roleClaim);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/create")]
        public async Task<IActionResult> CreateRoleClaim([FromBody] DataSecurityRoleClaimRequestDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return BadRequest("The submitted object is null.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newRoleClaim = await _roleClaimService.Create(createDto);

                return CreatedAtAction(nameof(GetRoleClaimById), new { id = newRoleClaim.Id }, newRoleClaim);
            }
            catch(InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPut("v1/update/{id}")]
        public async Task<IActionResult> UpdateRoleClaim(int id, [FromBody] DataSecurityRoleClaimRequestDto updateDto)
        {
            try
            {
                if (updateDto == null)
                {
                    return BadRequest("The submitted object is null.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                var updatedRoleClaim = await _roleClaimService.Update(id, updateDto);

                return Ok(updatedRoleClaim);
            }
            catch(System.Collections.Generic.KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }
    }
}
