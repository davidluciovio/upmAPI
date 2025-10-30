using Entity.Dtos;
using LogicDomain;
using Microsoft.AspNetCore.Mvc;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataProductionModelController : Controller
    {
        private readonly DataProductionModelService _dataProductionModelService;
        public DataProductionModelController(DataProductionModelService dataProductionModelService) 
        {
            _dataProductionModelService = dataProductionModelService;
        }

        [HttpPost("v1/create")]
        public async Task<IActionResult> CreateProductionModel([FromBody] DataProductionModelCreateDto modelCreateDto)
        {
            try
            {
                if (modelCreateDto == null)
                {
                    return BadRequest("El objeto enviado es nulo.");
                }

                var newModel = await _dataProductionModelService.CreateProductionModel(modelCreateDto);

                return CreatedAtAction(nameof(GetProductionModelById), new { id = newModel.Id }, newModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet("v1/get-all")]
        public async Task<ActionResult> GetAllProductionModels()
        {
            try
            {
                var models = await _dataProductionModelService.GetAllProductionModels();
                return Ok(models);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }


        [HttpGet("v1/get-id/{id}")]
        public async Task<IActionResult> GetProductionModelById(Guid id)
        {
            try
            {
                var model = await _dataProductionModelService.GetProductionModelById(id);
                if (model == null)
                {
                    return NotFound();
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
