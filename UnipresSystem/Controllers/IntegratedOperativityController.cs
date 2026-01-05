using Entity.AplicationDtos._03_IntegratedOperativity;
using LogicDomain.ApplicationServices;
using Microsoft.AspNetCore.Mvc;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IntegratedOperativityController : ControllerBase
    {
        private readonly IntegratedOperativityService _integratedOperativityService;

        public IntegratedOperativityController(IntegratedOperativityService integratedOperativityService)
        {
            _integratedOperativityService = integratedOperativityService;
        }

        [HttpPost("v1/post-integrated-operativity")]
        public async Task<IActionResult> FilterData([FromBody] IntegratedOperativityRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _integratedOperativityService.FilterDataCombined(request);
            return Ok(result);
        }
    }
}
