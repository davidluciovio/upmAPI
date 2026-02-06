using Entity.Dtos._01_Auth.DataSecurityPermission;
using Entity.Interfaces;
using Entity.Models.Auth;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicDomain._00_DataUPM
{
    public class DataSecurityPermissionService : IService<DataSecurityPermissionResponseDto, DataSecurityPermissionRequestDto>
    {
        private readonly AuthContext _authContext;

        public DataSecurityPermissionService(AuthContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<DataSecurityPermissionResponseDto> Create(DataSecurityPermissionRequestDto dtocreate)
        {
            if (await _authContext.Permissions.AnyAsync(p => p.Permission == dtocreate.Permission && p.SubmoduleId == dtocreate.SubmoduleId))
            {
                throw new InvalidOperationException($"Permission with name '{dtocreate.Permission}' already exists for this submodule.");
            }

            var permission = new AuthPermissions
            {
                Id = Guid.NewGuid(),
                Permission = dtocreate.Permission,
                Clave = dtocreate.Clave,
                SubmoduleId = dtocreate.SubmoduleId,
                CreateBy = dtocreate.CreateBy,
                Active = true,
                CreateDate = DateTime.UtcNow
            };

            _authContext.Permissions.Add(permission);
            await _authContext.SaveChangesAsync();

            return new DataSecurityPermissionResponseDto
            {
                Id = permission.Id,
                Permission = permission.Permission,
                Clave = permission.Clave,
                SubmoduleId = permission.SubmoduleId
            };
        }

        public async Task<List<DataSecurityPermissionResponseDto>> GetAlls()
        {
            return await _authContext.Permissions
                .Where(p => p.Active)
                .Select(p => new DataSecurityPermissionResponseDto
                {
                    Id = p.Id,
                    Active = p.Active,
                    Permission = p.Permission,
                    Clave = p.Clave,
                    SubmoduleId = p.SubmoduleId
                }).ToListAsync();
        }

        public async Task<DataSecurityPermissionResponseDto?> GetById(Guid id)
        {
            var permission = await _authContext.Permissions.FirstOrDefaultAsync(p => p.Id == id && p.Active);

            if (permission == null)
            {
                return null;
            }

            return new DataSecurityPermissionResponseDto
            {
                Id = permission.Id,
                Permission = permission.Permission,
                Clave = permission.Clave,
                SubmoduleId = permission.SubmoduleId
            };
        }

        public async Task<DataSecurityPermissionResponseDto> Update(Guid id, DataSecurityPermissionRequestDto dtoUpdate)
        {
            var permission = await _authContext.Permissions.FindAsync(id);

            if (permission == null)
            {
                throw new KeyNotFoundException($"Permission with ID '{id}' not found.");
            }

            if (await _authContext.Permissions.AnyAsync(p => p.Id != id && p.Permission == dtoUpdate.Permission && p.SubmoduleId == dtoUpdate.SubmoduleId))
            {
                throw new InvalidOperationException($"Another permission with name '{dtoUpdate.Permission}' already exists for this submodule.");
            }

            permission.Permission = dtoUpdate.Permission;
            permission.Clave = dtoUpdate.Clave;
            permission.SubmoduleId = dtoUpdate.SubmoduleId;
            permission.Active = dtoUpdate.Active;

            _authContext.Permissions.Update(permission);
            await _authContext.SaveChangesAsync();

            return new DataSecurityPermissionResponseDto
            {
                Id = permission.Id,
                Permission = permission.Permission,
                Clave = permission.Clave,
                SubmoduleId = permission.SubmoduleId
            };
        }
    }
}