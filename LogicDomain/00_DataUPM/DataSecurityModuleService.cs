using Entity.Dtos._00_DataUPM.DataSecurityModule;
using Entity.Interfaces;
using Entity.Models.Auth; // AuthModule is here
using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicDomain._00_DataUPM
{
    public class DataSecurityModuleService : IService<DataSecurityModuleResponseDto, DataSecurityModuleRequestDto>
    {
        private readonly AuthContext _authContext;

        public DataSecurityModuleService(AuthContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<DataSecurityModuleResponseDto> Create(DataSecurityModuleRequestDto dtocreate)
        {
            if (await _authContext.Modules.AnyAsync(m => m.Module == dtocreate.Module))
            {
                throw new InvalidOperationException($"Module with name '{dtocreate.Module}' already exists.");
            }

            var module = new AuthModule
            {
                Module = dtocreate.Module,
                Icon = dtocreate.Icon,
                Route = dtocreate.Route,
                CreateBy = dtocreate.CreateBy,
                Active = true,
                CreateDate = DateTime.UtcNow
            };

            _authContext.Modules.Add(module);
            await _authContext.SaveChangesAsync();

            return new DataSecurityModuleResponseDto
            {
                Id = module.Id,
                Module = module.Module,
                Icon = module.Icon,
                Route = module.Route,
                Active = module.Active,
            };
        }

        public async Task<List<DataSecurityModuleResponseDto>> GetAlls()
        {
            return await _authContext.Modules
                .Where(m => m.Active)
                .Select(m => new DataSecurityModuleResponseDto
                {
                    Id = m.Id,
                    Module = m.Module,
                    Icon = m.Icon,
                    Route = m.Route,
                    Active = m.Active,
                }).ToListAsync();
        }

        public async Task<DataSecurityModuleResponseDto?> GetById(Guid id)
        {
            var module = await _authContext.Modules.FirstOrDefaultAsync(m => m.Id == id && m.Active);

            if (module == null)
            {
                return null;
            }

            return new DataSecurityModuleResponseDto
            {
                Id = module.Id,
                Module = module.Module,
                Icon = module.Icon,
                Route = module.Route,
                Active = module.Active
            };
        }

        public async Task<DataSecurityModuleResponseDto> Update(Guid id, DataSecurityModuleRequestDto dtoUpdate)
        {
            var module = await _authContext.Modules.FindAsync(id);

            if (module == null)
            {
                throw new KeyNotFoundException($"Module with ID '{id}' not found.");
            }

            if (await _authContext.Modules.AnyAsync(m => m.Id != id && m.Module == dtoUpdate.Module))
            {
                throw new InvalidOperationException($"Another module with name '{dtoUpdate.Module}' already exists.");
            }

            module.Module = dtoUpdate.Module;
            module.Icon = dtoUpdate.Icon;
            module.Route = dtoUpdate.Route;
            // Assuming CreateBy and CreateDate are not updated here.
            // Active status might be updated via a separate method or handled here if needed.

            await _authContext.SaveChangesAsync();

            return new DataSecurityModuleResponseDto
            {
                Id = module.Id,
                Module = module.Module,
                Icon = module.Icon,
                Route = module.Route,
                Active = module.Active
            };
        }
    }
}
