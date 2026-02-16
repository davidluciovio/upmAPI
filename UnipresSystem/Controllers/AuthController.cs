using Entity.Dtos._01_Auth.DataAuth;
using Entity.Dtos.Auth;
using Entity.Models.Auth;
using LogicDomain;
using LogicDomain.SystemServices;
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
        private readonly LdapService _ldapService;
        private readonly UserManager<AuthUser> _userManager;


        public AuthController(AuthService authService, UserManager<AuthUser> userManager, LdapService ldapService)
        {
            _authService = authService;
            _ldapService = ldapService;
            _userManager = userManager;
        }

        [HttpPost("login-ldap")]
        public async Task<IActionResult> Login_LDAP([FromBody] AuthLoginDto model)
        {
            // --- 1. INTENTO CON ACTIVE DIRECTORY ---
            try
            {
                var ldapData = await _ldapService.AuthenticateAndGetDetails(model.CodeUser, model.Password);

                if (ldapData != null)
                {
                    var user = await _userManager.FindByNameAsync(model.CodeUser);

                    if (user == null)
                    {
                        user = new AuthUser
                        {
                            UserName = model.CodeUser,
                            Email = ldapData.Email,
                            PrettyName = ldapData.DisplayName,
                            CreateDate = DateTime.Now,
                            CreateBy = "LDAP_Sync",
                            Active = true
                        };
                        await _userManager.CreateAsync(user);
                        await _userManager.AddToRoleAsync(user, "Usuario");
                    }
                    else
                    {
                        // Sincronización de datos (siempre útil)
                        user.PrettyName = ldapData.DisplayName;
                        user.Email = ldapData.Email;
                        await _userManager.UpdateAsync(user);
                    }

                    var tokenLdap = await _authService.LoginAuthenticatedUser(model.CodeUser);
                    return Ok(new { token = tokenLdap, user = new { user.UserName, user.PrettyName }, source = "AD" });
                }
            }
            catch (Exception)
            {
                /* Si falla LDAP por red o configuración, permitimos que intente local */
            }

            // --- 2. PLAN B: USUARIO LOCAL (ADMINISTRADORES) ---
            try
            {
                // Usamos tu método Login que ya tienes en AuthService
                var tokenLocal = await _authService.Login(model.CodeUser, model.Password);

                // Obtenemos los datos para la respuesta
                var localUser = await _userManager.FindByNameAsync(model.CodeUser)
                                 ?? await _userManager.FindByEmailAsync(model.CodeUser);

                return Ok(new { token = tokenLocal, user = new { localUser.UserName, localUser.PrettyName }, source = "Local" });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Credenciales inválidas.");
            }
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
