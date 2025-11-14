using Entity.Dtos.DataProduction;
using LogicDomain.DataProduction;
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

        [HttpPost("v1/desactivate")]
        public async Task<IActionResult> SetProductionModelDesactivate([FromBody] Guid id)
        {
            try
            {
                var desactivate = await _dataProductionModelService.DeactivateProductionModel(id);
                if (!desactivate) throw new Exception($"Error al desactivar el modelo con id: {id}");

                return Ok(desactivate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/activate")]
        public async Task<IActionResult> SetProductionModelActivate([FromBody] Guid id)
        {
            try
            {
                var activate = await _dataProductionModelService.ActivateProductionModel(id);
                if (!activate) throw new Exception($"Error al activar el modelo con id: {id}");

                return Ok(activate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error en sistema {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/update/{id}")]
        public async Task<IActionResult> SetProductionModelUpdate(Guid id, [FromBody] DataProductionModelDto model)
        {
            try
            {
                var updatedModel = await _dataProductionModelService.UpdateProductionModel(id, model);

                if (updatedModel == null) throw new Exception($"Error al activar el modelo con id: {id}");

                return Ok(updatedModel);
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
