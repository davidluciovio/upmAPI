using Entity.Dtos.DataProduction;
using Entity.Models.DataProduction;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicDomain.DataProduction
{
    public class DataProductionLocationService
    {
        private readonly DataContext _dataContext;

        public DataProductionLocationService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<DataProductionLocationDto> CreateProductionLocation(DataProductionLocationCreateDto newLocation)
        {
            var location = new DataProductionLocation
            {
                Active = true,
                CreateBy = newLocation.CreateBy,
                CreateDate = DateTime.Now,
                Id = Guid.NewGuid(),
                LocationDescription = newLocation.LocationDescription
            };
            var response = _dataContext.ProductionLocations.Add(location);
            await _dataContext.SaveChangesAsync();

            return new DataProductionLocationDto
            {
                Active = response.Entity.Active,
                CreateBy = response.Entity.CreateBy,
                CreateDate = response.Entity.CreateDate,
                Id = response.Entity.Id,
                LocationDescription = response.Entity.LocationDescription
            };
        }

        public async Task<List<DataProductionLocationDto>> GetAllProductionLocations()
        {
            var locations = await _dataContext.ProductionLocations.ToListAsync();
            return locations.Select(location => new DataProductionLocationDto
            {
                Id = location.Id,
                Active = location.Active,
                CreateBy = location.CreateBy,
                CreateDate = location.CreateDate,
                LocationDescription = location.LocationDescription
            }).ToList();
        }
        public async Task<DataProductionLocationDto?> GetProductionLocationById(Guid id)
        {
            var location = await _dataContext.ProductionLocations.FindAsync(id);
            if (location == null)
            {
                return null;
            }
            return new DataProductionLocationDto
            {
                Id = location.Id,
                Active = location.Active,
                CreateBy = location.CreateBy,
                CreateDate = location.CreateDate,
                LocationDescription = location.LocationDescription
            };
        }

        public async Task<bool> DeactivateProductionLocation(Guid id)
        {
            var Location = await _dataContext.ProductionLocations.FindAsync(id);
            if (Location == null)
            {
                return false;
            }
            Location.Active = false;
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateProductionLocation(Guid id)
        {
            var Location = await _dataContext.ProductionLocations.FindAsync(id);
            if (Location == null)
            {
                return false;
            }
            Location.Active = true;
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<DataProductionLocationDto> UpdateProductionLocation(Guid id, DataProductionLocationDto locationDto)
        {
            var location = await _dataContext.ProductionLocations.FindAsync(id);
            if (location == null)
            {
                throw new KeyNotFoundException($"ProductionLocation with Id '{id}' not found.");
            }

            location.Active = locationDto.Active;
            location.LocationDescription = locationDto.LocationDescription;
            location.CreateBy = locationDto.CreateBy;
            location.CreateDate = DateTime.Now;

            await _dataContext.SaveChangesAsync();

            return new DataProductionLocationDto
            {
                Id = location.Id,
                Active = location.Active,
                CreateBy = location.CreateBy,
                CreateDate = location.CreateDate,
                LocationDescription = location.LocationDescription
            };
        }

    }
}
