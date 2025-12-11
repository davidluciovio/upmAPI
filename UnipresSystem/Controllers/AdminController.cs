using Entity.Dtos.Auth;
using LogicDomain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "SuperAdmin")]
    public class AdminController : Controller
    {
        //private readonly AdminService _adminService;
        //public AdminController(AdminService adminService)
        //{
        //    _adminService = adminService;
        //}

        //[HttpPost("create-module")]
        //public async Task<IActionResult> CreateModulo([FromBody] AuthCreateModuloDto newModule)
        //{
        //    try
        //    { 
        //        var module = await _adminService.CreateModule(newModule);
        //        return CreatedAtAction(nameof(module), new { id = module.Id, module = module.Module }, module);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new {message = $"No se pudo crear el modulo {ex.Message}" });
        //    }
        //}

        public IActionResult Index()
        {
            return View();
        }
    }
}
