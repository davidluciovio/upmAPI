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
    public class PartNumberAreaService : IServiceCrud<PartNumberAreaDto, PartNumberAreaCreateDto, PartNumberAreaUpdateDto>
    {
        private readonly ProductionControlContext _productionControlContext;
        private readonly DataContext _dataContext;

        public PartNumberAreaService(ProductionControlContext productionControlContext, DataContext dataContext) 
        {
            _productionControlContext = productionControlContext;
            _dataContext = dataContext;
        }

        public async Task<PartNumberAreaDto> Create(PartNumberAreaCreateDto createDto)
        {
            var partNumberArea = new PartNumberArea
            {
                PartNumberId = createDto.PartNumberId,
                AreaId = createDto.AreaId,
                Active = true,
                CreateBy = createDto.CreateBy,
                CreateDate = DateTime.UtcNow,
                UpdateBy = createDto.UpdateBy,
                UpdateDate = DateTime.UtcNow
            };
            _productionControlContext.PartNumberAreas.Add(partNumberArea);
            await _productionControlContext.SaveChangesAsync();

            return new PartNumberAreaDto
            {
                Id = partNumberArea.Id,
                PartNumber = partNumberArea.PartNumberId.ToString(),
                Area = partNumberArea.AreaId.ToString(),
                Active = partNumberArea.Active,
                CreateBy = partNumberArea.CreateBy,
                CreateDate = partNumberArea.CreateDate,
                UpdateBy = partNumberArea.UpdateBy,
                UpdateDate = partNumberArea.UpdateDate
            };
        }

      
        public async Task<List<PartNumberAreaDto>> GetAlls()
        {
            var partNumberAreas = await _productionControlContext.PartNumberAreas.ToListAsync();

            return partNumberAreas.Select(pna => new PartNumberAreaDto
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
                Active = pna.Active,
                CreateBy = pna.CreateBy,
                CreateDate = pna.CreateDate,
                UpdateBy = pna.UpdateBy,
                UpdateDate = pna.UpdateDate
            }).ToList();
        }

        public async Task<PartNumberAreaDto?> GetById(Guid id)
        {
            var partNumberArea = await _productionControlContext.PartNumberAreas.FindAsync(id);
            if (partNumberArea == null)
            {
                throw new KeyNotFoundException("PartNumberArea not found");
            }
            return new PartNumberAreaDto
            {
                Id = partNumberArea.Id,
                PartNumber = _dataContext.ProductionPartNumbers
                    .Where(pn => pn.Id == partNumberArea.PartNumberId)
                    .Select(pn => pn.PartNumberName)
                    .FirstOrDefault() ?? partNumberArea.PartNumberId.ToString(),
                Area = _dataContext.ProductionAreas
                    .Where(a => a.Id == partNumberArea.AreaId)
                    .Select(a => a.AreaDescription)
                    .FirstOrDefault() ?? partNumberArea.AreaId.ToString(),
                Active = partNumberArea.Active,
                CreateBy = partNumberArea.CreateBy,
                CreateDate = partNumberArea.CreateDate,
                UpdateBy = partNumberArea.UpdateBy,
                UpdateDate = partNumberArea.UpdateDate
            };
        }

        public async Task<PartNumberAreaDto> Update(Guid id, PartNumberAreaUpdateDto updateDto)
        {
            var partNumberArea = _productionControlContext.PartNumberAreas.Find(id);
            if (partNumberArea == null) throw new KeyNotFoundException("PartNumberArea not found");

            var parNumber = _dataContext.ProductionPartNumbers
                .Find(updateDto.PartNumberId);
            if (parNumber == null) throw new KeyNotFoundException("PartNumber not found");

            var area = _dataContext.ProductionAreas
                .Find(updateDto.AreaId);
            if (area == null) throw new KeyNotFoundException("Area not found");

            partNumberArea.Active = updateDto.Active;
            partNumberArea.PartNumberId = parNumber.Id;
            partNumberArea.AreaId = area.Id;
            partNumberArea.UpdateBy = updateDto.UpdateBy;
            partNumberArea.UpdateDate = DateTime.UtcNow;

            _productionControlContext.PartNumberAreas.Update(partNumberArea);
            await _productionControlContext.SaveChangesAsync();

            return new PartNumberAreaDto
            {
                Id = partNumberArea.Id,
                PartNumber = parNumber.PartNumberName,
                Area = area.AreaDescription,
                Active = partNumberArea.Active,
                CreateBy = partNumberArea.CreateBy,
                CreateDate = partNumberArea.CreateDate,
                UpdateBy = partNumberArea.UpdateBy,
                UpdateDate = partNumberArea.UpdateDate
            };
        }
    }
}
