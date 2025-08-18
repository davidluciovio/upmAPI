using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upmData.Context;
using upmData.Models;

namespace upmDomain.UserTools
{
    public class UserService
    {
        private readonly UpmwebContext _context;
        public UserService(UpmwebContext context) 
        {
            _context = context;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = _context.Users
                .Where(user => user.Active)
                .Select(user => UserMapper.Map(user));

            return await users.ToListAsync();
        }

        public async Task<int> AddAsync(UserDto user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // Validar duplicado por Email
            var exists = await _context.Users
                .AnyAsync(u => u.CodeUser == user.CodeUser);

            if (exists)
                throw new InvalidOperationException("La nomina ya está registrada.");

            var newUser = new User
            {
                Active = true,
                CodeUser = user.CodeUser,
                CreateBy = user.CreateBy,
                CreateDate = DateTime.UtcNow, // mejor usar UTC para consistencia
                Email = user.UserEmail ?? "".Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password),
                UserName = user.UserName?.Trim() ?? string.Empty
            };

            await _context.Users.AddAsync(newUser);
            

            return await _context.SaveChangesAsync();
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            return new UserDto
            {
                CodeUser = user.CodeUser,
                CreateBy = user.CreateBy,
                UserEmail = user.Email,
                UserName = user.UserName
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;
            user.Active = false;
            await UpdateAsync(UserMapper.Map(user));
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> UpdateAsync(UserDto user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == user.UserId);

            if (existingUser == null)
                throw new KeyNotFoundException($"No se encontró el usuario con Id {user.UserId}");

            // Actualizar propiedades
            existingUser.CodeUser = user.CodeUser;
            existingUser.Email = user.UserEmail;
            existingUser.UserName = user.UserName ?? existingUser.UserName;
            existingUser.Active = user.Active;
            existingUser.CreateDate = DateTime.Now;

            // Solo actualizar contraseña si se envía
            if (!string.IsNullOrEmpty(user.Password))
            {
                existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }

            _context.Users.Update(existingUser);
            return await _context.SaveChangesAsync();
        }
    }
}
