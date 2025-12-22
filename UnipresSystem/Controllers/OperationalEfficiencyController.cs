using Entity.AplicationDtos._02_OperationalEfficiencyDtos;
using LogicDomain.ApplicationServices;
using Microsoft.AspNetCore.Mvc;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperationalEfficiencyController : ControllerBase
    {
        private readonly OperationalEfficiencyService _operationalEfficiencyService;

        public OperationalEfficiencyController(OperationalEfficiencyService operationalEfficiencyService)
        {
            _operationalEfficiencyService = operationalEfficiencyService;
        }

        [HttpPost("v1/post-grouped-production")]
        public async Task<IActionResult> PostGroupedProduction([FromBody] OperationalEfficiencyRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _operationalEfficiencyService.GetGroupedProductionAsync(request);
            return Ok(result);
        }
    }
}
