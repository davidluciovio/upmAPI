using Entity.Dtos._01_Auth.DataAuth;
using Entity.Dtos.Auth;
using Entity.Models.Auth;
using LogicData.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LogicDomain
{
    public class AuthService
    {
        private readonly AuthContext _authContext;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AuthUser> _userManager;
        private readonly RoleManager<AuthRole> _roleManager;


        public AuthService(AuthContext authContext, IConfiguration configuration, UserManager<AuthUser> userManager, RoleManager<AuthRole> roleManager)
        {
            _authContext = authContext;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> Register(AuthRegisterDto registerDto)
        {
            var userExists = await _userManager.FindByEmailAsync(registerDto.Email);
            if (userExists != null)
            {
                throw new Exception($"Ya existe un usuario con el codigo {registerDto.Email}");
            }

            var user = new AuthUser
            {
                Active = true,
                CreateDate = DateTime.UtcNow,
                CreateBy = registerDto.CreateBy,
                UpdateDate = DateTime.UtcNow,
                UpdateBy = registerDto.CreateBy,
                Email = registerDto.Email,
                UserName = registerDto.UserName ?? registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString() // Necesario para Identity
                
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                // Devuelve los errores de Identity (ej. "password no cumple requisitos")
                throw new Exception(JsonSerializer.Serialize(result.Errors));
            }

            if (await _roleManager.RoleExistsAsync("Usuario"))
            {
                await _userManager.AddToRoleAsync(user, "Usuario");
            }

            return "Usuario registrado exitosamente";
        }

        public async Task<string> LoginAuthenticatedUser(string codeUser)
        {
            var user = await _userManager.FindByNameAsync(codeUser);

            if (user == null) throw new UnauthorizedAccessException("Usuario no encontrado para token generation."); // Should not happen if LDAP auth was successful and user was synced

            // 1. Obtener claims base (ID, email...)
            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Name, user.PrettyName!),
            };

            // 2. Obtener roles del usuario (ej. ["Vendedor", "Contador"])
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var roleName in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, roleName));

                // 3. ¡LA MAGIA! Obtener los permisos (claims) de CADA rol
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);

                    // Agregamos solo los claims que sean de tipo "Permiso"
                    var permisosClaims = roleClaims.Where(c => c.Type == "Permiso");
                    authClaims.AddRange(permisosClaims);
                }
            }

            // 4. Generar el JWT con TODOS los claims (roles y permisos)
            if (string.IsNullOrEmpty(_configuration["Jwt:Issuer"]) || string.IsNullOrEmpty(_configuration["Jwt:Audience"]))
            {
                throw new Exception("Configuración JWT incompleta en el servidor.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: authClaims,
                expires: DateTime.Now.AddHours(10),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> Login(string codeUser, string password)
        {
            var user = await _userManager.FindByEmailAsync(codeUser);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password)) throw new UnauthorizedAccessException("Credenciales Incorrectas");

            // 1. Obtener claims base (ID, email...)
            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Name, user.PrettyName!),
            };

            // 2. Obtener roles del usuario (ej. ["Vendedor", "Contador"])
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var roleName in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, roleName));

                // 3. ¡LA MAGIA! Obtener los permisos (claims) de CADA rol
                var role = await _roleManager.FindByNameAsync(roleName);
                var roleClaims = await _roleManager.GetClaimsAsync(role!);

                // Agregamos solo los claims que sean de tipo "Permiso"
                var permisosClaims = roleClaims.Where(c => c.Type == "Permiso");
                authClaims.AddRange(permisosClaims);
            }

            // 4. Generar el JWT con TODOS los claims (roles y permisos)
            if (string.IsNullOrEmpty(_configuration["Jwt:Issuer"]) || string.IsNullOrEmpty(_configuration["Jwt:Audience"]))
            {
                throw new Exception("Configuración JWT incompleta en el servidor.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: authClaims,
                expires: DateTime.Now.AddHours(10),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public async Task<AuthUserDto> Authenticate(string codeUser, string password)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(u => u.UserName == codeUser);
            if (user == null) return null!;
            var userDto = new AuthUserDto()
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty
            };

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) throw new UnauthorizedAccessException("Credenciales Incorrectas");

            return userDto;
        } 

    }
}
