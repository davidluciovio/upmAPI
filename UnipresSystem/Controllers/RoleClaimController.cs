using Entity.Dtos._01_Auth.RoleClaim;
using Entity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleClaimController : ControllerBase
    {
        private readonly IRoleClaimService _roleClaimService;

        public RoleClaimController(IRoleClaimService roleClaimService)
        {
            _roleClaimService = roleClaimService;
        }

        [HttpGet("v1/get-by-role/{roleId}")]
        public async Task<IActionResult> GetClaimsByRoleId(string roleId)
        {
            try
            {
                var claims = await _roleClaimService.GetClaimsByRoleId(roleId);
                return Ok(claims);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/create")]
        public async Task<IActionResult> AddClaimToRole([FromBody] RoleClaimRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("The submitted object is null.");
                }

                var newClaim = await _roleClaimService.AddClaimToRole(request);
                
                return Ok(newClaim);
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

        [HttpDelete("v1/delete/{claimId}")]
        public async Task<IActionResult> RemoveClaimFromRole(int claimId)
        {
            try
            {
                var result = await _roleClaimService.RemoveClaimFromRole(claimId);
                if (result)
                {
                    return NoContent();
                }
                return NotFound();
            }
            catch (KeyNotFoundException ex)
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
