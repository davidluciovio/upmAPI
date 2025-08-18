using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using upmData.Context;
using upmData.Models;

namespace upmDomain.Auth
{
    public class AuthService
    {
        private readonly UpmwebContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(UpmwebContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> Authenticate(string userCode, string password)
        {
            var user = ValidateCredentials(userCode, password);
            if (user == null ) return null!;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, $"{user.Email}"),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var roles = await GetRoles(user);


            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Description));
            }

            if (string.IsNullOrEmpty(_configuration["Jwt:Issuer"]) || string.IsNullOrEmpty(_configuration["Jwt:Audience"]))
            {
                throw new Exception("Configuración JWT incompleta en el servidor.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User? ValidateCredentials(string userCode, string password)
        {
            var user = _context.Users.FirstOrDefault(user => user.CodeUser == userCode);
            if (user == null) return null;
            if(BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) throw new UnauthorizedAccessException("Credenciales Incorrectas");
            return user;
        }

        private async Task<List<Role>> GetRoles(User user)
        {
            return await _context.UserConfigurations
                .Where(uc => uc.UserId == user.Id)
                .Select(uc => uc.Role)
                .ToListAsync();
        }

      

    }
}
