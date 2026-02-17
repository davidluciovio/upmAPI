using Entity.Dtos.ModelDtos.ProductionControl.PartNumberStructure;
using Entity.Interfaces;
using Entity.Models.ProductionControl;
using LogicData.Context;
using LogicDomain.ProductionControl;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicDomain.ModelServices.ProductionControl
{
    public class PartNumberStructureService : IService<PartNumberStructureResponseDto, PartNumberStructureRequestDto>
    {
        private readonly ProductionControlContext _context;
        private readonly MaterialSupplierService _materialSupplierService;
        private readonly PartNumberLogisticsService _partNumberLogisticsService;

        public PartNumberStructureService(
            ProductionControlContext context,
            MaterialSupplierService materialSupplierService,
            PartNumberLogisticsService partNumberLogisticsService)
        {
            _context = context;
            _materialSupplierService = materialSupplierService;
            _partNumberLogisticsService = partNumberLogisticsService;
        }

        public async Task<PartNumberStructureResponseDto> Create(PartNumberStructureRequestDto createDto)
        {
            // Validation for duplicate entries
            if (await _context.PartNumberStructures.AnyAsync(pne =>
                pne.PartNumberLogisticId == createDto.PartNumberLogisticId &&
                pne.CompletePartId == createDto.CompletePartId &&
                pne.MaterialSuplierId == createDto.MaterialSuplierId))
            {
                throw new InvalidOperationException($"PartNumberStructure with PartNumberLogisticId '{createDto.PartNumberLogisticId}', CompletePartId '{createDto.CompletePartId}' and MaterialSuplierId '{createDto.MaterialSuplierId}' already exists.");
            }

            var newPartNumberStructure = new PartNumberStructure
            {
                PartNumberLogisticId = createDto.PartNumberLogisticId,
                CompletePartId = createDto.CompletePartId,
                CompletePartName = createDto.CompletePartName,
                Quantity = createDto.Quantity,
                MaterialSuplierId = createDto.MaterialSuplierId,
                CreateBy = createDto.CreateBy,
                Active = true,
                CreateDate = DateTime.UtcNow
            };

            _context.PartNumberStructures.Add(newPartNumberStructure);
            await _context.SaveChangesAsync();

            return await MapToDto(newPartNumberStructure);
        }

        public async Task<bool> Delete(Guid id)
        {
            var partNumberStructure = await _context.PartNumberStructures.FindAsync(id);
            if (partNumberStructure == null)
            {
                return false;
            }

            partNumberStructure.Active = false; // Soft delete
            partNumberStructure.UpdateDate = DateTime.UtcNow;
            // No update by field for delete based on MaterialSupplierService delete method

            _context.Entry(partNumberStructure).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<PartNumberStructureResponseDto>> GetAlls()
        {
            var partNumberStructures = await _context.PartNumberStructures
                .Where(pne => pne.Active)
                .ToListAsync();

            var materialSupplierIds = partNumberStructures.Select(pns => pns.MaterialSuplierId).Distinct().ToList();
            var partNumberLogisticIds = partNumberStructures.Select(pns => pns.PartNumberLogisticId).Distinct().ToList();

            var materialSuppliers = await _context.MaterialSuppliers
                .Where(ms => materialSupplierIds.Contains(ms.Id))
                .ToDictionaryAsync(ms => ms.Id);

            var partNumberLogistics = await _context.partNumberLogistics
                .Where(pnl => partNumberLogisticIds.Contains(pnl.Id))
                .ToDictionaryAsync(pnl => pnl.Id);

            return partNumberStructures.Select(pns => new PartNumberStructureResponseDto
            {
                Id = pns.Id,
                Active = pns.Active,
                CreateDate = pns.CreateDate,
                CreateBy = pns.CreateBy,
                UpdateDate = pns.UpdateDate,
                UpdateBy = pns.UpdateBy,
                PartNumberLogisticId = pns.PartNumberLogisticId,
                CompletePartId = pns.CompletePartId,
                CompletePartName = pns.CompletePartName,
                Quantity = pns.Quantity,
                MaterialSuplierId = pns.MaterialSuplierId,
                MaterialSupplierDescription = materialSuppliers.TryGetValue(pns.MaterialSuplierId, out var supplier) ? supplier.MaterialSupplierDescription : "N/A",
                PartNumberLogisticDescription = partNumberLogistics.TryGetValue(pns.PartNumberLogisticId, out var logistic) ? logistic.PartNumberId.ToString() : "N/A"
            }).ToList();
        }

        public async Task<PartNumberStructureResponseDto?> GetById(Guid id)
        {
            var partNumberStructure = await _context.PartNumberStructures
                .FirstOrDefaultAsync(pne => pne.Id == id && pne.Active);

            return partNumberStructure != null ? await MapToDto(partNumberStructure) : null;
        }

        public async Task<PartNumberStructureResponseDto> Update(Guid id, PartNumberStructureRequestDto updateDto)
        {
            var partNumberStructure = await _context.PartNumberStructures.FindAsync(id);
            if (partNumberStructure == null)
            {
                throw new KeyNotFoundException($"PartNumberStructure with ID '{id}' not found.");
            }

            // Validation for duplicate entries, excluding the current entity
            if (await _context.PartNumberStructures.AnyAsync(pne =>
                pne.Id != id &&
                pne.PartNumberLogisticId == updateDto.PartNumberLogisticId &&
                pne.CompletePartId == updateDto.CompletePartId &&
                pne.MaterialSuplierId == updateDto.MaterialSuplierId))
            {
                throw new InvalidOperationException($"Another PartNumberStructure with PartNumberLogisticId '{updateDto.PartNumberLogisticId}', CompletePartId '{updateDto.CompletePartId}' and MaterialSuplierId '{updateDto.MaterialSuplierId}' already exists.");
            }

            partNumberStructure.PartNumberLogisticId = updateDto.PartNumberLogisticId;
            partNumberStructure.CompletePartId = updateDto.CompletePartId;
            partNumberStructure.CompletePartName = updateDto.CompletePartName;
            partNumberStructure.Quantity = updateDto.Quantity;
            partNumberStructure.MaterialSuplierId = updateDto.MaterialSuplierId;
            partNumberStructure.Active = updateDto.Active;
            partNumberStructure.UpdateDate = DateTime.UtcNow;
            partNumberStructure.UpdateBy = updateDto.UpdateBy;

            await _context.SaveChangesAsync();

            return await MapToDto(partNumberStructure);
        }

        private async Task<PartNumberStructureResponseDto> MapToDto(PartNumberStructure partNumberStructure)
        {
            var materialSupplier = await _materialSupplierService.GetById(partNumberStructure.MaterialSuplierId);
            // Assuming PartNumberLogisticsService GetById returns a DTO that has a description.
            var partNumberLogistic = await _partNumberLogisticsService.GetById(partNumberStructure.PartNumberLogisticId);

            return new PartNumberStructureResponseDto
            {
                Id = partNumberStructure.Id,
                Active = partNumberStructure.Active,
                CreateDate = partNumberStructure.CreateDate,
                CreateBy = partNumberStructure.CreateBy,
                UpdateDate = partNumberStructure.UpdateDate,
                UpdateBy = partNumberStructure.UpdateBy,
                PartNumberLogisticId = partNumberStructure.PartNumberLogisticId,
                CompletePartId = partNumberStructure.CompletePartId,
                CompletePartName = partNumberStructure.CompletePartName,
                Quantity = partNumberStructure.Quantity,
                MaterialSuplierId = partNumberStructure.MaterialSuplierId,
                MaterialSupplierDescription = materialSupplier?.MaterialSupplierDescription ?? "N/A",
                PartNumberLogisticDescription = partNumberLogistic?.PartNumber ?? "N/A"
            };
        }
    }
}
