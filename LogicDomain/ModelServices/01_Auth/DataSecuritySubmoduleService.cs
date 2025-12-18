using Entity.Dtos._01_Auth.DataSecuritySubmodule;
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
    public class DataSecuritySubmoduleService : IService<DataSecuritySubmoduleResponseDto, DataSecuritySubmoduleRequestDto>
    {
        private readonly AuthContext _authContext;

        public DataSecuritySubmoduleService(AuthContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<DataSecuritySubmoduleResponseDto> Create(DataSecuritySubmoduleRequestDto dtocreate)
        {
            if (await _authContext.Submodules.AnyAsync(s => s.Submodule == dtocreate.Submodule && s.ModuleId == dtocreate.ModuleId))
            {
                throw new InvalidOperationException($"Submodule with name '{dtocreate.Submodule}' already exists in this module.");
            }

            var submodule = new AuthSubmodule
            {
                Submodule = dtocreate.Submodule,
                ModuleId = dtocreate.ModuleId,
                Icon = dtocreate.Icon,
                Route = dtocreate.Route,
                CreateBy = dtocreate.CreateBy,
                Active = true,
                CreateDate = DateTime.UtcNow
            };

            _authContext.Submodules.Add(submodule);
            await _authContext.SaveChangesAsync();

            return new DataSecuritySubmoduleResponseDto
            {
                Id = submodule.Id,
                Submodule = submodule.Submodule,
                ModuleId = submodule.ModuleId,
                Icon = submodule.Icon,
                Route = submodule.Route,
                Active = submodule.Active,
            };
        }

        public async Task<List<DataSecuritySubmoduleResponseDto>> GetAlls()
        {
            return await _authContext.Submodules
                .Where(s => s.Active)
                .Select(s => new DataSecuritySubmoduleResponseDto
                {
                    Id = s.Id,
                    Submodule = s.Submodule,
                    ModuleId = s.ModuleId,
                    Icon = s.Icon,
                    Route = s.Route,
                    Active = s.Active,
                }).ToListAsync();
        }

        public async Task<DataSecuritySubmoduleResponseDto?> GetById(Guid id)
        {
            var submodule = await _authContext.Submodules.FirstOrDefaultAsync(s => s.Id == id && s.Active);

            if (submodule == null)
            {
                return null;
            }

            return new DataSecuritySubmoduleResponseDto
            {
                Id = submodule.Id,
                Submodule = submodule.Submodule,
                ModuleId = submodule.ModuleId,
                Icon = submodule.Icon,
                Route = submodule.Route,
                Active = submodule.Active
            };
        }

        public async Task<DataSecuritySubmoduleResponseDto> Update(Guid id, DataSecuritySubmoduleRequestDto dtoUpdate)
        {
            var submodule = await _authContext.Submodules.FindAsync(id);

            if (submodule == null)
            {
                throw new KeyNotFoundException($"Submodule with ID '{id}' not found.");
            }

            if (await _authContext.Submodules.AnyAsync(s => s.Id != id && s.Submodule == dtoUpdate.Submodule && s.ModuleId == dtoUpdate.ModuleId))
            {
                throw new InvalidOperationException($"Another submodule with name '{dtoUpdate.Submodule}' already exists in this module.");
            }

            submodule.Submodule = dtoUpdate.Submodule;
            submodule.ModuleId = dtoUpdate.ModuleId;
            submodule.Icon = dtoUpdate.Icon;
            submodule.Route = dtoUpdate.Route;
            
            await _authContext.SaveChangesAsync();

            return new DataSecuritySubmoduleResponseDto
            {
                Id = submodule.Id,
                Submodule = submodule.Submodule,
                ModuleId = submodule.ModuleId,
                Icon = submodule.Icon,
                Route = submodule.Route,
                Active = submodule.Active
            };
        }
    }
}