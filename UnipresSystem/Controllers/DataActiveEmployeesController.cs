using Entity.Models.DataUPM;
using LogicDomain.ModelServices.DataProduction;
using Microsoft.AspNetCore.Mvc;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataActiveEmployeesController : ControllerBase
    {
        private readonly DataActiveEmployeesService _dataActiveEmployeesService;

        public DataActiveEmployeesController(DataActiveEmployeesService dataActiveEmployeesService)
        {
            _dataActiveEmployeesService = dataActiveEmployeesService;
        }

        [HttpGet("v1/get-all")]
        public async Task<ActionResult<List<DataActiveEmployees>>> GetActiveEmployees()
        {
            var activeEmployees = await _dataActiveEmployeesService.GetActiveEmployees();
            return Ok(activeEmployees);
        }
    }
}
