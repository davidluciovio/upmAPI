using Entity.Dtos;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicDomain
{
    public class DataProductionAreaService
    {
        private readonly DataContext _dataContext;

        public DataProductionAreaService(DataContext dataContext) 
        {
            _dataContext = dataContext;
        }

        public async Task<DataProductionAreaDto> CreateProductionArea(DataProductionAreaCreateDto areaDto)
        {
            var area = new Entity.Models.DataProductionArea
            {
                Active = true,
                CreateBy = areaDto.CreateBy,
                CreateDate = DateTime.Now,
                Id = Guid.NewGuid(),
                AreaDescription = areaDto.AreaDescription
            };
            _dataContext.ProductionAreas.Add(area);
            await _dataContext.SaveChangesAsync();
            return new DataProductionAreaDto
            {
                Id = area.Id,
                Active = area.Active,
                CreateBy = area.CreateBy,
                CreateDate = area.CreateDate,
                AreaDescription = area.AreaDescription
            };
        }

        public async Task<List<DataProductionAreaDto>> GetAllProductionAreas()
        {
            var areas = await _dataContext.ProductionAreas.ToListAsync();
            return areas.Select(area => new DataProductionAreaDto
            {
                Id = area.Id,
                Active = area.Active,
                CreateBy = area.CreateBy,
                CreateDate = area.CreateDate,
                AreaDescription = area.AreaDescription
            }).ToList();
        }

        public async Task<DataProductionAreaDto?> GetProductionAreaById(Guid id)
        {
            var area = await _dataContext.ProductionAreas.FindAsync(id);
            if (area == null)
            {
                return null;
            }
            return new DataProductionAreaDto
            {
                Id = area.Id,
                Active = area.Active,
                CreateBy = area.CreateBy,
                CreateDate = area.CreateDate,
                AreaDescription = area.AreaDescription
            };
        }

        public async Task<bool> DeactivateProductionArea(Guid id)
        {
            var area = await _dataContext.ProductionAreas.FindAsync(id);
            if (area == null)
            {
                return false;
            }
            area.Active = false;
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateProductionArea(Guid id)
        {
            var area = await _dataContext.ProductionAreas.FindAsync(id);
            if (area == null)
            {
                return false;
            }
            area.Active = true;
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<DataProductionAreaDto> UpdateProductionArea(Guid id, DataProductionAreaDto areaDto)
        {
            var area = await _dataContext.ProductionAreas.FindAsync(id);
            if (area == null)
            {
                throw new KeyNotFoundException($"ProductionArea with Id '{id}' not found.");
            }

            area.Active = areaDto.Active;
            area.AreaDescription = areaDto.AreaDescription;
            area.CreateBy = areaDto.CreateBy;
            area.CreateDate = DateTime.Now;

            await _dataContext.SaveChangesAsync();

            return new DataProductionAreaDto
            {
                Id = area.Id,
                Active = area.Active,
                CreateBy = area.CreateBy,
                CreateDate = area.CreateDate,
                AreaDescription = area.AreaDescription
            };
        }
    }
}