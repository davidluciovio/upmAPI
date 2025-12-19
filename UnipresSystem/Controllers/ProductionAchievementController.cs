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

        [HttpPost]
        public async Task<IActionResult> GetProductionAchievement([FromBody] ProductionAchievementRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productionAchievementService.GetProductionAchievement(request);
            return Ok(result);
        }
    }
}
