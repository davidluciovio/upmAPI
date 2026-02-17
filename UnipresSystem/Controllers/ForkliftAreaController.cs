using Entity.Dtos.ModelDtos.ProductionControl.ForkliftArea;
using LogicDomain.ModelServices.ProductionControl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForkliftAreaController : ControllerBase
    {
        private readonly ForkliftAreaService _service;

        public ForkliftAreaController(ForkliftAreaService service)
        {
            _service = service;
        }

        [HttpGet("{userId}/areas")]
        public async Task<IActionResult> GetAreasForUser(Guid userId)
        {
            var areas = await _service.GetForkliftAreasByUserIdAsync(userId);
            if (areas == null || areas.Count == 0)
            {
                return NotFound($"No areas found for user with ID {userId}.");
            }
            return Ok(areas);
        }

        [HttpPost("{userId}/areas")]
        public async Task<IActionResult> AssignAreasToUser(Guid userId, [FromBody] ForkliftAreaRequestDto request)
        {
            if (request == null || request.DataProductionAreaIds == null || request.DataProductionAreaIds.Count == 0)
            {
                return BadRequest("A list of production area IDs must be provided.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.AddForkliftAreaAsync(userId, request.DataProductionAreaIds);
                return Ok(new { message = "Areas assigned successfully." });
            }
            catch (Exception ex)
            {
                // Log the exception (logging not shown)
                return StatusCode(500, "An internal error occurred while assigning areas.");
            }
        }
    }
}
