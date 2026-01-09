using Entity.AplicationDtos._01_ProductionAcvhievementDtos;
using LogicDomain.ApplicationServices;
using Microsoft.AspNetCore.Mvc;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductionAchievementController : ControllerBase
    {
        private readonly ProductionAchievementService _productionAchievementService;

        public ProductionAchievementController(ProductionAchievementService productionAchievementService)
        {
            _productionAchievementService = productionAchievementService;
        }

        [HttpPost("v1/post-production-achievement")]
        public async Task<IActionResult> GetProductionAchievement([FromBody] ProductionAchievementRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productionAchievementService.GetProductionAchievement(request);
            return Ok(result);
        }

        [HttpPost("v1/get-processed-achievement")]
        public async Task<IActionResult> GetProcessedAchievement([FromBody] ProductionAchievementRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productionAchievementService.GetProcessedAchievementAsync(request);
            return Ok(result);
        }
    }
}
