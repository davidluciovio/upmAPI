using Entity.Dtos._00_DataUPM;
using Entity.Interfaces;
using Entity.Models.Auth;
using LogicData.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LogicDomain._00_DataUPM
{
    public class DataSecurityUserService : IServiceCrud<DataSecurityUserDto, DataSecurityUserCreateDto, DataSecurityUserUpdateDto>
    {
        private readonly AuthContext _authContext;
        private readonly IPasswordHasher<AuthUser> _passwordHasher;
        private readonly UserManager<AuthUser> _userManager;
        private readonly RoleManager<AuthRole> _roleManager;

        public DataSecurityUserService(AuthContext authContext, IPasswordHasher<AuthUser> passwordHasher, UserManager<AuthUser> userManager, RoleManager<AuthRole> roleManager)
        {
            _authContext = authContext;
            _passwordHasher = passwordHasher;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<DataSecurityUserDto> Create(DataSecurityUserCreateDto createDto)
        {
            if (await _authContext.Users.AnyAsync(u => u.UserName == createDto.UserName))
            {
                throw new InvalidOperationException($"Username '{createDto.UserName}' already exists.");
            }
            if (await _authContext.Users.AnyAsync(u => u.Email == createDto.Email))
            {
                throw new InvalidOperationException($"Email '{createDto.Email}' is already in use.");
            }
            var role = await _authContext.Roles.FindAsync(createDto.RoleId);

            if (role == null)
            {
                throw new KeyNotFoundException("Role not found");
            }

            var user = new AuthUser
            {
                UserName = string.Concat("UPM",createDto.Email.ToUpper()),
                Email = createDto.Email,
                PrettyName = createDto.PrettyName.ToUpper(),
                Active = true,
                CreateBy = createDto.CreateBy,
                UpdateBy = createDto.CreateBy, // Initially same as CreateBy
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            var newUser = await _userManager.CreateAsync(user, createDto.Password);

            user.PasswordHash = _passwordHasher.HashPassword(user, createDto.Password);

            _authContext.Users.Add(user);
            if (!newUser.Succeeded)
            {
                // Devuelve los errores de Identity (ej. "password no cumple requisitos")
                throw new Exception(JsonSerializer.Serialize(newUser.Errors));
            }

            if (await _roleManager.RoleExistsAsync(role.Name ?? "Usuario"))
            {
                await _userManager.AddToRoleAsync(user, role.Name ?? "Usuario");
            }

            return new DataSecurityUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Active = user.Active,
                RoleName = role.Name ?? "Usuario",
                CreateBy = user.CreateBy,
                CreateDate = user.CreateDate,
                UpdateBy = user.UpdateBy,
                UpdateDate = user.UpdateDate
            };
        }

        public async Task<List<DataSecurityUserDto>> GetAlls()
        {
            var users = await _authContext.Users
                .Join(_authContext.UserRoles,
                    u => u.Id, 
                    ur => ur.UserId, 
                    (u, ur) => new { u, ur })
                .Join(_authContext.Roles,
                    r=> r.ur.RoleId,
                    role => role.Id,
                    (r, role) => new DataSecurityUserDto
                    {
                        Id = r.u.Id,
                        UserName = r.u.UserName,
                        Email = r.u.Email,
                        Active = r.u.Active,
                        CreateBy = r.u.CreateBy,
                        CreateDate = r.u.CreateDate,
                        UpdateBy = r.u.UpdateBy,
                        UpdateDate = r.u.UpdateDate,
                        RoleName = role.Name,
                        PrettyName = r.u.PrettyName
                    }).ToListAsync();

            return users.Select(user => new DataSecurityUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                PrettyName = user.PrettyName,
                Email = user.Email,
                Active = user.Active,
                RoleName = user.RoleName ?? "N/A",
                CreateBy = user.CreateBy,
                CreateDate = user.CreateDate,
                UpdateBy = user.UpdateBy,
                UpdateDate = user.UpdateDate
            }).ToList();
        }

        public async Task<DataSecurityUserDto?> GetById(Guid id)
        {
            var user = await _authContext.Users
                .Join(_authContext.UserRoles,
                    u => u.Id,
                    ur => ur.UserId,
                    (u, ur) => new { u, ur })
                .Join(_authContext.Roles,
                    r => r.ur.RoleId,
                    role => role.Id,
                    (r, role) => new DataSecurityUserDto
                    {
                        Id = r.u.Id,
                        UserName = r.u.UserName,
                        PrettyName = r.u.PrettyName,
                        Email = r.u.Email,
                        Active = r.u.Active,
                        CreateBy = r.u.CreateBy,
                        CreateDate = r.u.CreateDate,
                        UpdateBy = r.u.UpdateBy,
                        UpdateDate = r.u.UpdateDate,
                        RoleName = role.Name
                    })
                .FirstOrDefaultAsync(u => u.Id == id.ToString());

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            return new DataSecurityUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                PrettyName = user.PrettyName,
                Email = user.Email,
                Active = user.Active,
                RoleName = user.RoleName ?? "N/A",
                CreateBy = user.CreateBy,
                CreateDate = user.CreateDate,
                UpdateBy = user.UpdateBy,
                UpdateDate = user.UpdateDate
            };
        }

        public async Task<DataSecurityUserDto> Update(Guid id, DataSecurityUserUpdateDto updateDto)
        {
            var user = await _authContext.Users.FindAsync(id.ToString());
            var userRole = await _authContext.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == id.ToString());

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            if (await _authContext.Users.AnyAsync(u => u.Id != id.ToString() && u.UserName == updateDto.UserName))
            {
                throw new InvalidOperationException($"Username '{updateDto.UserName}' already exists.");
            }
            if (await _authContext.Users.AnyAsync(u => u.Id != id.ToString() && u.Email == updateDto.Email))
            {
                throw new InvalidOperationException($"Email '{updateDto.Email}' is already in use.");
            }
            var role = await _authContext.Roles.FindAsync(updateDto.RoleId.ToString());
            if (role == null)
            {
                throw new KeyNotFoundException("Role not found");
            }

            user.UserName = string.Concat("UPM", updateDto.Email.ToUpper());
            user.NormalizedUserName = string.Concat("UPM", updateDto.Email.ToUpper());
            user.Email = updateDto.Email;
            user.NormalizedEmail = updateDto.Email.ToUpper();
            user.PrettyName = updateDto.PrettyName.ToUpper();
            user.Active = updateDto.Active;
            user.UpdateBy = updateDto.UpdateBy;
            user.UpdateDate = DateTime.UtcNow;

            user.PasswordHash = _passwordHasher.HashPassword(user, updateDto.Password);

            _authContext.Users.Update(user);

            if (userRole != null)
            {
                userRole.RoleId = updateDto.RoleId.ToString();
                _authContext.UserRoles.Update(userRole);
            }
            await _authContext.SaveChangesAsync();

            return new DataSecurityUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                PrettyName = user.PrettyName,
                Email = user.Email,
                Active = user.Active,
                RoleName = role.Name,
                CreateBy = user.CreateBy,
                CreateDate = user.CreateDate,
                UpdateBy = user.UpdateBy,
                UpdateDate = user.UpdateDate
            };
        }
    }
}
