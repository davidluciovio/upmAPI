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
    public class PartNumberLogisticsService : IServiceCrud<PartNumberLogisticsDto, PartNumberLogisticsCreateDto, PartNumberLogisticsUpdateDto>
    {
        private readonly ProductionControlContext _productionControlContext;
        private readonly DataContext _dataContext;

        public PartNumberLogisticsService(ProductionControlContext productionControlContext, DataContext dataContext) 
        {
            _productionControlContext = productionControlContext;
            _dataContext = dataContext;
        }

        public async Task<PartNumberLogisticsDto> Create(PartNumberLogisticsCreateDto createDto)
        {
            var partNumberLogistic = new PartNumberLogistics
            {
                PartNumberId = createDto.PartNumberId,
                AreaId = createDto.AreaId,
                LocationId = createDto.LocationId,
                SNP = createDto.SNP,
                Active = true,
                CreateBy = createDto.CreateBy,
                CreateDate = DateTime.UtcNow,
                UpdateBy = createDto.UpdateBy,
                UpdateDate = DateTime.UtcNow
            };
            _productionControlContext.partNumberLogistics.Add(partNumberLogistic);
            await _productionControlContext.SaveChangesAsync();

            return new PartNumberLogisticsDto
            {
                Id = partNumberLogistic.Id,
                PartNumber = partNumberLogistic.PartNumberId.ToString(),
                Area = partNumberLogistic.AreaId.ToString(),
                Location = partNumberLogistic.LocationId.ToString(),
                SNP = partNumberLogistic.SNP,
                Active = partNumberLogistic.Active,
                CreateBy = partNumberLogistic.CreateBy,
                CreateDate = partNumberLogistic.CreateDate,
                UpdateBy = partNumberLogistic.UpdateBy,
                UpdateDate = partNumberLogistic.UpdateDate
            };
        }

      
        public async Task<List<PartNumberLogisticsDto>> GetAlls()
        {
            var partNumberLogistics = await _productionControlContext.partNumberLogistics.ToListAsync();

            return partNumberLogistics.Select(pna => new PartNumberLogisticsDto
            {
                Id = pna.Id,
                PartNumber = _dataContext.ProductionPartNumbers
                    .Where(pn => pn.Id == pna.PartNumberId)
                    .Select(pn => pn.PartNumberName)
                    .FirstOrDefault() ?? pna.PartNumberId.ToString(),
                Area = _dataContext.ProductionAreas
                    .Where(a => a.Id == pna.AreaId)
                    .Select(a=> a.AreaDescription)
                    .FirstOrDefault() ?? pna.AreaId.ToString(),
                Location = _dataContext.ProductionLocations
                    .Where(l => l.Id == pna.LocationId)
                    .Select(l => l.LocationDescription)
                    .FirstOrDefault() ?? pna.LocationId.ToString(),
                SNP = pna.SNP,
                Active = pna.Active,
                CreateBy = pna.CreateBy,
                CreateDate = pna.CreateDate,
                UpdateBy = pna.UpdateBy,
                UpdateDate = pna.UpdateDate
            }).ToList();
        }

        public async Task<PartNumberLogisticsDto?> GetById(Guid id)
        {
            var partNumberLogistic = await _productionControlContext.partNumberLogistics.FindAsync(id);
            if (partNumberLogistic == null)
            {
                throw new KeyNotFoundException("partNumberLogistic not found");
            }
            return new PartNumberLogisticsDto
            {
                Id = partNumberLogistic.Id,
                PartNumber = _dataContext.ProductionPartNumbers
                    .Where(pn => pn.Id == partNumberLogistic.PartNumberId)
                    .Select(pn => pn.PartNumberName)
                    .FirstOrDefault() ?? partNumberLogistic.PartNumberId.ToString(),
                Area = _dataContext.ProductionAreas
                    .Where(a => a.Id == partNumberLogistic.AreaId)
                    .Select(a => a.AreaDescription)
                    .FirstOrDefault() ?? partNumberLogistic.AreaId.ToString(),
                Location = _dataContext.ProductionLocations
                    .Where(l => l.Id == partNumberLogistic.LocationId)
                    .Select(l => l.LocationDescription)
                    .FirstOrDefault() ?? partNumberLogistic.LocationId.ToString(),
                SNP = partNumberLogistic.SNP,
                Active = partNumberLogistic.Active,
                CreateBy = partNumberLogistic.CreateBy,
                CreateDate = partNumberLogistic.CreateDate,
                UpdateBy = partNumberLogistic.UpdateBy,
                UpdateDate = partNumberLogistic.UpdateDate
            };
        }

        public async Task<PartNumberLogisticsDto> Update(Guid id, PartNumberLogisticsUpdateDto updateDto)
        {
            var partNumberLogistic = _productionControlContext.partNumberLogistics.Find(id);
            if (partNumberLogistic == null) throw new KeyNotFoundException("partNumberLogistic not found");

            var parNumber = _dataContext.ProductionPartNumbers
                .Find(updateDto.PartNumberId);
            if (parNumber == null) throw new KeyNotFoundException("PartNumber not found");

            var area = _dataContext.ProductionAreas
                .Find(updateDto.AreaId);
            if (area == null) throw new KeyNotFoundException("Area not found");

            var location = _dataContext.ProductionLocations
                .Find(updateDto.LocationId);
            if (location == null) throw new KeyNotFoundException("PartNumber not found");

            partNumberLogistic.Active = updateDto.Active;
            partNumberLogistic.PartNumberId = parNumber.Id;
            partNumberLogistic.AreaId = area.Id;
            partNumberLogistic.LocationId = updateDto.LocationId;
            partNumberLogistic.SNP = updateDto.SNP;
            partNumberLogistic.UpdateBy = updateDto.UpdateBy;
            partNumberLogistic.UpdateDate = DateTime.UtcNow;

            _productionControlContext.partNumberLogistics.Update(partNumberLogistic);
            await _productionControlContext.SaveChangesAsync();

            return new PartNumberLogisticsDto
            {
                Id = partNumberLogistic.Id,
                PartNumber = parNumber.PartNumberName,
                Area = area.AreaDescription,
                Location = location.LocationDescription,
                SNP = partNumberLogistic.SNP,
                Active = partNumberLogistic.Active,
                CreateBy = partNumberLogistic.CreateBy,
                CreateDate = partNumberLogistic.CreateDate,
                UpdateBy = partNumberLogistic.UpdateBy,
                UpdateDate = partNumberLogistic.UpdateDate
            };
        }
    }
}
