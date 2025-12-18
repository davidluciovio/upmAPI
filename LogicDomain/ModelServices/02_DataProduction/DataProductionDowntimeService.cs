
using Entity.Interfaces;
using Entity.ModelDtos._02_DataProduction.DataProductionDowntime;
using Entity.Models.DataProduction; // Added this
using LogicData.Context;
using Microsoft.EntityFrameworkCore; // Added this
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicDomain.ModelServices._02_DataProduction
{
    public class DataProductionDowntimeService : IService<DataProductionDowntimeResponseDto, DataProductionDowntimeRequestDto>
    {
        private readonly DataContext _dataContext;

        public DataProductionDowntimeService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<DataProductionDowntimeResponseDto> Create(DataProductionDowntimeRequestDto dtocreate)
        {
            if (await _dataContext.ProductionDowntimes.AnyAsync(s => s.DowntimeDescription == dtocreate.DowntimeDescription))
            {
                throw new InvalidOperationException($"Downtime with description '{dtocreate.DowntimeDescription}' already exists.");
            }

            var downtime = new DataProductionDowntime
            {
                Id = Guid.NewGuid(),
                DowntimeDescription = dtocreate.DowntimeDescription,
                InforCode = dtocreate.InforCode,
                PLCCode = dtocreate.PLCCode,
                IsDirectDowntime = dtocreate.IsDirectDowntime,
                Programable = dtocreate.Programable,
                CreateBy = dtocreate.CreateBy,
                Active = true,
                CreateDate = DateTime.UtcNow
            };

            _dataContext.ProductionDowntimes.Add(downtime);
            await _dataContext.SaveChangesAsync();

            return new DataProductionDowntimeResponseDto
            {
                Id = downtime.Id,
                DowntimeDescription = downtime.DowntimeDescription,
                InforCode = downtime.InforCode,
                PLCCode = downtime.PLCCode,
                IsDirectDowntime = downtime.IsDirectDowntime,
                Programable = downtime.Programable,
                Active = downtime.Active
            };
        }

        public async Task<List<DataProductionDowntimeResponseDto>> GetAlls()
        {
            return await _dataContext.ProductionDowntimes
                .Where(s => s.Active)
                .Select(s => new DataProductionDowntimeResponseDto
                {
                    Id = s.Id,
                    DowntimeDescription = s.DowntimeDescription,
                    InforCode = s.InforCode,
                    PLCCode = s.PLCCode,
                    IsDirectDowntime = s.IsDirectDowntime,
                    Programable = s.Programable,
                    Active = s.Active,
                }).ToListAsync();
        }

        public async Task<DataProductionDowntimeResponseDto?> GetById(Guid id)
        {
            var downtime = await _dataContext.ProductionDowntimes.FirstOrDefaultAsync(s => s.Id == id && s.Active);

            if (downtime == null)
            {
                return null;
            }

            return new DataProductionDowntimeResponseDto
            {
                Id = downtime.Id,
                DowntimeDescription = downtime.DowntimeDescription,
                InforCode = downtime.InforCode,
                PLCCode = downtime.PLCCode,
                IsDirectDowntime = downtime.IsDirectDowntime,
                Programable = downtime.Programable,
                Active = downtime.Active
            };
        }

        public async Task<DataProductionDowntimeResponseDto> Update(Guid id, DataProductionDowntimeRequestDto dtoUpdate)
        {
            var downtime = await _dataContext.ProductionDowntimes.FindAsync(id);

            if (downtime == null)
            {
                throw new KeyNotFoundException($"Downtime with ID '{id}' not found.");
            }

            if (await _dataContext.ProductionDowntimes.AnyAsync(s => s.Id != id && s.DowntimeDescription == dtoUpdate.DowntimeDescription))
            {
                throw new InvalidOperationException($"Another downtime with description '{dtoUpdate.DowntimeDescription}' already exists.");
            }

            downtime.DowntimeDescription = dtoUpdate.DowntimeDescription;
            downtime.InforCode = dtoUpdate.InforCode;
            downtime.PLCCode = dtoUpdate.PLCCode;
            downtime.IsDirectDowntime = dtoUpdate.IsDirectDowntime;
            downtime.Programable = dtoUpdate.Programable;
            
            await _dataContext.SaveChangesAsync();

            return new DataProductionDowntimeResponseDto
            {
                Id = downtime.Id,
                DowntimeDescription = downtime.DowntimeDescription,
                InforCode = downtime.InforCode,
                PLCCode = downtime.PLCCode,
                IsDirectDowntime = downtime.IsDirectDowntime,
                Programable = downtime.Programable,
                Active = downtime.Active
            };
        }
    }
}
