using Entity.Dtos.ProductionControl;
using Entity.Interfaces;
using Entity.Models.ProductionControl;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicDomain.ProductionControl
{
    public class PartNumberLocationService : IServiceCrud<PartNumberLocationDto, PartNumberLocationCreateDto, PartNumberLocationUpdateDto>
    {
        private readonly ProductionControlContext _productionControlContext;
        private readonly DataContext _dataContext;

        public PartNumberLocationService(ProductionControlContext productionControlContext, DataContext dataContext)
        {
            _productionControlContext = productionControlContext;
            _dataContext = dataContext;
        }

        public async Task<PartNumberLocationDto> Create(PartNumberLocationCreateDto createDto)
        {
            var partNumberLocation = new PartNumberLocation
            {
                PartNumberId = createDto.PartNumberId,
                LocationId = createDto.LocationId,
                Active = true,
                CreateBy = createDto.CreateBy,
                CreateDate = DateTime.UtcNow,
                UpdateBy = createDto.UpdateBy,
                UpdateDate = DateTime.UtcNow
            };
            _productionControlContext.PartNumberLocations.Add(partNumberLocation);
            await _productionControlContext.SaveChangesAsync();

            var partNumberName = await _dataContext.ProductionPartNumbers
                .Where(pn => pn.Id == partNumberLocation.PartNumberId)
                .Select(pn => pn.PartNumberName)
                .FirstOrDefaultAsync() ?? partNumberLocation.PartNumberId.ToString();

            var locationName = await _dataContext.ProductionLocations
                .Where(l => l.Id == partNumberLocation.LocationId)
                .Select(l => l.LocationDescription)
                .FirstOrDefaultAsync() ?? partNumberLocation.LocationId.ToString();

            return new PartNumberLocationDto
            {
                Id = partNumberLocation.Id,
                PartNumber = partNumberName,
                Location = locationName,
                Active = partNumberLocation.Active,
                CreateBy = partNumberLocation.CreateBy,
                CreateDate = partNumberLocation.CreateDate,
                UpdateBy = partNumberLocation.UpdateBy,
                UpdateDate = partNumberLocation.UpdateDate
            };
        }


        public async Task<List<PartNumberLocationDto>> GetAlls()
        {
            var partNumberLocations = await _productionControlContext.PartNumberLocations.ToListAsync();

            return partNumberLocations.Select(pnl => new PartNumberLocationDto
            {
                Id = pnl.Id,
                PartNumber = _dataContext.ProductionPartNumbers
                    .Where(pn => pn.Id == pnl.PartNumberId)
                    .Select(pn => pn.PartNumberName)
                    .FirstOrDefault() ?? pnl.PartNumberId.ToString(),
                Location = _dataContext.ProductionLocations
                    .Where(a => a.Id == pnl.LocationId)
                    .Select(a => a.LocationDescription)
                    .FirstOrDefault() ?? pnl.LocationId.ToString(),
                Active = pnl.Active,
                CreateBy = pnl.CreateBy,
                CreateDate = pnl.CreateDate,
                UpdateBy = pnl.UpdateBy,
                UpdateDate = pnl.UpdateDate
            }).ToList();
        }

        public async Task<PartNumberLocationDto?> GetById(Guid id)
        {
            var partNumberLocation = await _productionControlContext.PartNumberLocations.FindAsync(id);
            if (partNumberLocation == null)
            {
                throw new KeyNotFoundException("PartNumberLocation not found");
            }
            return new PartNumberLocationDto
            {
                Id = partNumberLocation.Id,
                PartNumber = _dataContext.ProductionPartNumbers
                    .Where(pn => pn.Id == partNumberLocation.PartNumberId)
                    .Select(pn => pn.PartNumberName)
                    .FirstOrDefault() ?? partNumberLocation.PartNumberId.ToString(),
                Location = _dataContext.ProductionLocations
                    .Where(a => a.Id == partNumberLocation.LocationId)
                    .Select(a => a.LocationDescription)
                    .FirstOrDefault() ?? partNumberLocation.LocationId.ToString(),
                Active = partNumberLocation.Active,
                CreateBy = partNumberLocation.CreateBy,
                CreateDate = partNumberLocation.CreateDate,
                UpdateBy = partNumberLocation.UpdateBy,
                UpdateDate = partNumberLocation.UpdateDate
            };
        }

        public async Task<PartNumberLocationDto> Update(Guid id, PartNumberLocationUpdateDto updateDto)
        {
            var partNumberLocation = await _productionControlContext.PartNumberLocations.FindAsync(id);
            if (partNumberLocation == null) throw new KeyNotFoundException("PartNumberLocation not found");

            var parNumber = await _dataContext.ProductionPartNumbers
                .FindAsync(updateDto.PartNumberId);
            if (parNumber == null) throw new KeyNotFoundException("PartNumber not found");

            var location = await _dataContext.ProductionLocations
                .FindAsync(updateDto.LocationId);
            if (location == null) throw new KeyNotFoundException("Location not found");

            partNumberLocation.Active = updateDto.Active;
            partNumberLocation.PartNumberId = parNumber.Id;
            partNumberLocation.LocationId = location.Id;
            partNumberLocation.UpdateBy = updateDto.UpdateBy;
            partNumberLocation.UpdateDate = DateTime.UtcNow;

            _productionControlContext.PartNumberLocations.Update(partNumberLocation);
            await _productionControlContext.SaveChangesAsync();

            return new PartNumberLocationDto
            {
                Id = partNumberLocation.Id,
                PartNumber = parNumber.PartNumberName,
                Location = location.LocationDescription,
                Active = partNumberLocation.Active,
                CreateBy = partNumberLocation.CreateBy,
                CreateDate = partNumberLocation.CreateDate,
                UpdateBy = partNumberLocation.UpdateBy,
                UpdateDate = partNumberLocation.UpdateDate
            };
        }
    }
}
