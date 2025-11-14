using Entity.Dtos.Auth;
using Entity.Models;
using Entity.Models.Auth;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicDomain
{
    public class AdminService
    {
        private readonly AuthContext _authContext;

        public AdminService(AuthContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<AuthModuleDto> CreateModule(AuthCreateModuloDto moduleDto)
        {
            var modulo = new AuthModule {
                Active = true,
                CreateBy = moduleDto.CreateBy,
                CreateDate = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                Module = moduleDto.Module
            };
            _authContext.Modules.Add(modulo);

            await _authContext.SaveChangesAsync();

            return new AuthModuleDto
            {
                Id = modulo.Id,
                Module = modulo.Module
            };
        }

        public async Task<AuthSubmoduleDto> CreateSubmodulo( AuthCreateSubmoduleDto submoduleDto)
        {
            var submodulo = new AuthSubmodule
            {
                Active = true,
                CreateBy = submoduleDto.CreateBy,
                CreateDate = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                ModuleId = submoduleDto.ModuleId,
                Submodule = submoduleDto.Submodule
            };
            _authContext.Submodulos.Add(submodulo);

            await _authContext.SaveChangesAsync();

            return new AuthSubmoduleDto
            {
                Id = submodulo.Id,
                Submodule = submodulo.Submodule,
                ModuleId = submodulo.ModuleId
            };
        }

        public async Task<AuthPermissionsDto> CreatePermiso(AuthCreatePermissionDto permissionDto)
        {
            var permission = new AuthPermissions
            {
                Active = true,
                Clave = permissionDto.Clave,
                CreateBy = permissionDto.CreateBy,
                CreateDate = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                Permission = permissionDto.Permission,
                SubmoduleId = permissionDto.SubmoduleId
            };
            _authContext.Permissions.Add(permission);
            await _authContext.SaveChangesAsync();

            return new AuthPermissionsDto
            {
                Id = permission.Id,
                Clave = permission.Clave,
                Permission = permission.Permission,
                SubmoduleId = permission.SubmoduleId
            };
        }

        public async Task<List<AuthModuleDto>> GetPermissionsTree()
        {
            var modules = await _authContext.Modules
                .Include(m => m.AuthSubmodules)
                    .ThenInclude(s => s.AuthPermissions)
                .AsNoTracking()
                .Select(m => new AuthModuleDto
                {
                    Id = m.Id, 
                    Module = m.Module,
                    Submodules = m.AuthSubmodules.Select(s => new AuthSubmoduleDto
                    {
                        Id = s.Id,
                        Submodule = s.Submodule,
                        ModuleId = s.ModuleId,
                        Permissions = s.AuthPermissions.Select(p => new AuthPermissionsDto
                        {
                            Id = p.Id,
                            Permission = p.Permission,
                            Clave = p.Clave,
                            SubmoduleId = p.SubmoduleId
                        }).ToList()
                    }).ToList()
                })
                .ToListAsync();

            

            return modules;
        }
    }
}
