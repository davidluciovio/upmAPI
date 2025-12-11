using Entity.Dtos.Auth;
using Entity.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "SuperAdmin")] // También protegido
    public class RolesAdminController : Controller
    {
        private readonly RoleManager<AuthRole> _roleManager;

        public RolesAdminController(RoleManager<AuthRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // --- CREAR ROLES ---
        //[HttpPost]
        //public async Task<IActionResult> CreateRole([FromBody] AuthCreateRoleDto dto)
        //{
        //    if (await _roleManager.RoleExistsAsync(dto.RoleName))
        //    {
        //        return BadRequest("El rol ya existe");
        //    }

        //    var role = new AuthRole { Name = dto.RoleName };
        //    var result = await _roleManager.CreateAsync(role);
         
        //    if (!result.Succeeded)
        //    {
        //        return BadRequest(result.Errors);
        //    }

        //    return Ok(new { Message = "Rol creado" });
        //}

        // --- LISTAR ROLES ---
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return Ok(roles);
        }

        // --- ASIGNAR PERMISOS A UN ROL ---
        // Este es el endpoint que "conecta" todo
        [HttpPost("{roleName}/permisos")]
        public async Task<IActionResult> AsignarPermisos(string roleName, [FromBody] List<string> permisosClave)
        {
            var rol = await _roleManager.FindByNameAsync(roleName);
            if (rol == null) return NotFound("Rol no encontrado");

            // Obtenemos todos los claims (permisos) actuales de ese rol
            var claimsActuales = await _roleManager.GetClaimsAsync(rol);
            var permisosActuales = claimsActuales.Where(c => c.Type == "Permiso");

            // 1. Borramos los permisos que ya no están en la lista nueva
            foreach (var claim in permisosActuales.Where(c => !permisosClave.Contains(c.Value)))
            {
                await _roleManager.RemoveClaimAsync(rol, claim);
            }

            // 2. Agregamos los permisos nuevos
            var claimsValuesActuales = permisosActuales.Select(c => c.Value);
            foreach (var clave in permisosClave.Where(c => !claimsValuesActuales.Contains(c)))
            {
                var nuevoClaim = new Claim("Permiso", clave);
                await _roleManager.AddClaimAsync(rol, nuevoClaim);
            }

            return Ok(new { Message = "Permisos actualizados para el rol" });
        }

        // --- VER PERMISOS DE UN ROL ---
        [HttpGet("{roleName}/permisos")]
        public async Task<IActionResult> GetPermisosDelRol(string roleName)
        {
            var rol = await _roleManager.FindByNameAsync(roleName);
            if (rol == null) return NotFound("Rol no encontrado");

            var claims = await _roleManager.GetClaimsAsync(rol);
            var permisos = claims
                .Where(c => c.Type == "Permiso")
                .Select(c => c.Value)
                .ToList();

            return Ok(permisos);
        }
    }
}
