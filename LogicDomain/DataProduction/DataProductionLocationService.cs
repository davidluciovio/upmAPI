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

        public async Task<List<DataProductionLocationDto>> GetAllProductionAreas()
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
    }
}
