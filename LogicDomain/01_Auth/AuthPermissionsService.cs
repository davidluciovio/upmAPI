using Entity.Dtos.Auth;
using Entity.Interfaces;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicDomain._01_Auth
{
    public class AuthPermissionsService : IService<AuthPermissionResponseDto, AuthCreatePermissionDto>
    {
        private readonly AuthContext _authContext;

        public AuthPermissionsService(AuthContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<AuthPermissionResponseDto> Create(AuthCreatePermissionDto dtocreate)
        {
            if(await _authContext.Permissions.AnyAsync(p => p.Clave == dtocreate.Clave))
            {
                throw new InvalidOperationException($"Permission with key '{dtocreate.Clave}' already exists.");
            }

            var permission = new AuthPermissions
            {
                Permission = dtocreate.Permission,
                Clave = dtocreate.Clave,
                SubmoduleId = dtocreate.SubmoduleId,
                CreateBy = dtocreate.CreateBy,
                Active = true,
                CreateDate = DateTime.UtcNow
            };

            _authContext.Permissions.Add(permission);
            await _authContext.SaveChangesAsync();

            return new AuthPermissionResponseDto
            {
                Id = permission.Id,
                Permission = permission.Permission,
                Clave = permission.Clave,
                SubmoduleId = permission.SubmoduleId
            };
        }

        public async Task<List<AuthPermissionResponseDto>> GetAlls()
        {
            return await _authContext.Permissions
                .Where(p => p.Active)
                .Select(p => new AuthPermissionResponseDto
                {
                    Id = p.Id,
                    Permission = p.Permission,
                    Clave = p.Clave,
                    SubmoduleId = p.SubmoduleId
                }).ToListAsync();
        }

        public async Task<AuthPermissionResponseDto?> GetById(Guid id)
        {
            var permission = await _authContext.Permissions.FirstOrDefaultAsync(p => p.Id == id && p.Active);

            if (permission == null)
            {
                return null;
            }

            return new AuthPermissionResponseDto
            {
                Id = permission.Id,
                Permission = permission.Permission,
                Clave = permission.Clave,
                SubmoduleId = permission.SubmoduleId
            };
        }

        public async Task<AuthPermissionResponseDto> Update(Guid id, AuthCreatePermissionDto dtoUpdate)
        {
            var permission = await _authContext.Permissions.FindAsync(id);

            if (permission == null)
            {
                throw new KeyNotFoundException($"Permission with ID '{id}' not found.");
            }

            if (await _authContext.Permissions.AnyAsync(p => p.Id != id && p.Clave == dtoUpdate.Clave))
            {
                throw new InvalidOperationException($"Another permission with key '{dtoUpdate.Clave}' already exists.");
            }

            permission.Permission = dtoUpdate.Permission;
            permission.Clave = dtoUpdate.Clave;
            permission.SubmoduleId = dtoUpdate.SubmoduleId;

            await _authContext.SaveChangesAsync();

            return new AuthPermissionResponseDto
            {
                Id = permission.Id,
                Permission = permission.Permission,
                Clave = permission.Clave,
                SubmoduleId = permission.SubmoduleId
            };
        }
    }
}
