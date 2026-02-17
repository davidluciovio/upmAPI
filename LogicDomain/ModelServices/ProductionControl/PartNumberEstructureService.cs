using Entity.Dtos.ModelDtos.ProductionControl.PartNumberEstructure;
using Entity.Interfaces;
using Entity.Models.ProductionControl;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicDomain.ModelServices.ProductionControl
{
    public class PartNumberEstructureService 
    {
        private readonly ProductionControlContext _context;
        private readonly string _user = "System"; // Consistent with existing services

        public PartNumberEstructureService(ProductionControlContext context)
        {
            _context = context;
        }

        public async Task<PartNumberEstructureResponseDto> Create(PartNumberEstructureRequestDto createDto)
        {
            var newPartNumberEstructure = new PartNumberEstructure
            {
                Id = Guid.NewGuid(),
                Active = createDto.Active,
                CreateDate = DateTime.UtcNow,
                CreateBy = createDto.CreateBy,
                UpdateDate = DateTime.UtcNow,
                UpdateBy = _user,
                PartNumberLogisticId = createDto.PartNumberLogisticId,
                CompletePartId = createDto.CompletePartId,
                CompletePartName = createDto.CompletePartName,
                Quantity = createDto.Quantity,
                MaterialSuplierId = createDto.MaterialSuplierId
            };

            _context.PartNumberEstructure.Add(newPartNumberEstructure);
            await _context.SaveChangesAsync();

            return MapToDto(newPartNumberEstructure);
        }

        public async Task<bool> Delete(Guid id)
        {
            var partNumberEstructure = await _context.PartNumberEstructure.FindAsync(id);
            if (partNumberEstructure == null)
            {
                return false;
            }

            partNumberEstructure.Active = false; // Soft delete
            partNumberEstructure.UpdateDate = DateTime.UtcNow;
            partNumberEstructure.UpdateBy = _user;
            _context.Entry(partNumberEstructure).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<PartNumberEstructureResponseDto>> GetAlls()
        {
            var partNumberEstructures = await _context.PartNumberEstructure.Where(pne => pne.Active).ToListAsync();
            return partNumberEstructures.Select(MapToDto).ToList();
        }

        public async Task<PartNumberEstructureResponseDto> GetById(Guid id)
        {
            var partNumberEstructure = await _context.PartNumberEstructure.FirstOrDefaultAsync(pne => pne.Id == id && pne.Active);
            return partNumberEstructure != null ? MapToDto(partNumberEstructure) : null;
        }

        public async Task<PartNumberEstructureResponseDto> Update(Guid id, PartNumberEstructureRequestDto updateDto)
        {
            var partNumberEstructure = await _context.PartNumberEstructure.FindAsync(id);
            if (partNumberEstructure == null)
            {
                throw new Exception("Part Number Estructure not found");
            }

            partNumberEstructure.Active = updateDto.Active;
            partNumberEstructure.PartNumberLogisticId = updateDto.PartNumberLogisticId;
            partNumberEstructure.CompletePartId = updateDto.CompletePartId;
            partNumberEstructure.CompletePartName = updateDto.CompletePartName;
            partNumberEstructure.Quantity = updateDto.Quantity;
            partNumberEstructure.MaterialSuplierId = updateDto.MaterialSuplierId;
            partNumberEstructure.UpdateDate = DateTime.UtcNow;
            partNumberEstructure.UpdateBy = updateDto.UpdateBy;

            _context.Entry(partNumberEstructure).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return MapToDto(partNumberEstructure);
        }

        private PartNumberEstructureResponseDto MapToDto(PartNumberEstructure partNumberEstructure)
        {
            return new PartNumberEstructureResponseDto
            {
                Id = partNumberEstructure.Id,
                Active = partNumberEstructure.Active,
                CreateDate = partNumberEstructure.CreateDate,
                CreateBy = partNumberEstructure.CreateBy,
                CompletePartName = partNumberEstructure.CompletePartName,
                Quantity = partNumberEstructure.Quantity,
            };
        }
    }
}
