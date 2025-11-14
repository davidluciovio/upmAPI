using Entity.Dtos.Auth;
using LogicDomain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginDto model)
        {
            try
            {
                return Ok(new { token = await _authService.Login(model.CodeUser, model.Password) } );
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRegisterDto registerDto)
        {
            try
            {
                return Ok(new { message = await _authService.Register(registerDto) } );
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
