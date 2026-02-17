using Entity.Dtos.AssyProduction;
using Entity.Dtos.ModelDtos.ProductionControl.PartNumberStructure;
using Entity.Dtos.ProductionControl;
using Entity.Interfaces;
using Entity.Models.ProductionControl;
using LogicData.Context;
using LogicDomain.AssyProduction;
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
        private readonly ProductionControlContext _contextProductionControl;
        private readonly AssyProductionContext _contextAssyProduction;
        private readonly MaterialSupplierService _materialSupplierService;
        private readonly PartNumberLogisticsService _partNumberLogisticsService;
        private readonly IServiceCrud<ProductionStationDto, ProductionStationCreateDto, ProductionStationUpdateDto> _productionStationService;

        public PartNumberStructureService(
            ProductionControlContext context,
            MaterialSupplierService materialSupplierService,
            PartNumberLogisticsService partNumberLogisticsService,
            IServiceCrud<ProductionStationDto, ProductionStationCreateDto, ProductionStationUpdateDto> productionStationService)
        {
            _contextProductionControl = context;
            _materialSupplierService = materialSupplierService;
            _partNumberLogisticsService = partNumberLogisticsService;
            _productionStationService = productionStationService;
        }

        public async Task<PartNumberStructureResponseDto> Create(PartNumberStructureRequestDto createDto)
        {
            // Validation for duplicate entries
            if (await _contextProductionControl.PartNumberStructures.AnyAsync(pne =>
                pne.PartNumberLogisticsId == createDto.PartNumberLogisticId &&
                pne.ProductionStationId == createDto.ProductionStationId &&
                pne.MaterialSuplierId == createDto.MaterialSuplierId))
            {
                throw new InvalidOperationException($"PartNumberStructure with PartNumberLogisticId '{createDto.PartNumberLogisticId}', ProductionStationId '{createDto.ProductionStationId}' and MaterialSuplierId '{createDto.MaterialSuplierId}' already exists.");
            }

            var newPartNumberStructure = new PartNumberStructure
            {
                Id = Guid.NewGuid(),
                Active = true,
                CreateBy = createDto.CreateBy,
                CreateDate = DateTime.UtcNow,
                UpdateBy = createDto.CreateBy,
                UpdateDate = DateTime.UtcNow,

                PartNumberName = createDto.PartNumberName,
                PartNumberDescription = createDto.PartNumberDescription,

                ProductionStationId = createDto.ProductionStationId,
                PartNumberLogisticsId = createDto.PartNumberLogisticId,
                PartNumberLogistics = await _contextProductionControl.PartNumberLogistics.FindAsync(createDto.PartNumberLogisticId),
                MaterialSuplierId = createDto.MaterialSuplierId,
                MaterialSupplier = await _contextProductionControl.MaterialSuppliers.FindAsync(createDto.MaterialSuplierId),
            };

            _contextProductionControl.PartNumberStructures.Add(newPartNumberStructure);
            await _contextProductionControl.SaveChangesAsync();

            return await MapToDto(newPartNumberStructure);
        }

        public async Task<bool> Delete(Guid id)
        {
            var partNumberStructure = await _contextProductionControl.PartNumberStructures.FindAsync(id);
            if (partNumberStructure == null)
            {
                return false;
            }

            partNumberStructure.Active = false; // Soft delete
            partNumberStructure.UpdateDate = DateTime.UtcNow;

            _contextProductionControl.Entry(partNumberStructure).State = EntityState.Modified;
            await _contextProductionControl.SaveChangesAsync();

            return true;
        }

        public async Task<List<PartNumberStructureResponseDto>> GetAlls()
        {
            var partNumberStructures = await _contextProductionControl.PartNumberStructures
                .Where(pne => pne.Active)
                .ToListAsync();

            var dtos = new List<PartNumberStructureResponseDto>();
            foreach (var pns in partNumberStructures)
            {
                dtos.Add(await MapToDto(pns));
            }

            return dtos;
        }

        public async Task<PartNumberStructureResponseDto?> GetById(Guid id)
        {
            var partNumberStructure = await _contextProductionControl.PartNumberStructures
                .FirstOrDefaultAsync(pne => pne.Id == id && pne.Active);

            return partNumberStructure != null ? await MapToDto(partNumberStructure) : null;
        }

        public async Task<PartNumberStructureResponseDto> Update(Guid id, PartNumberStructureRequestDto updateDto)
        {
            var partNumberStructure = await _contextProductionControl.PartNumberStructures.FindAsync(id);
            if (partNumberStructure == null)
            {
                throw new KeyNotFoundException($"PartNumberStructure with ID '{id}' not found.");
            }

            // Validation for duplicate entries, excluding the current entity
            if (await _contextProductionControl.PartNumberStructures.AnyAsync(pne =>
                pne.Id != id &&
                pne.PartNumberLogisticsId == updateDto.PartNumberLogisticId &&
                pne.ProductionStationId == updateDto.ProductionStationId &&
                pne.MaterialSuplierId == updateDto.MaterialSuplierId))
            {
                throw new InvalidOperationException($"Another PartNumberStructure with PartNumberLogisticId '{updateDto.PartNumberLogisticId}', ProductionStationId '{updateDto.ProductionStationId}' and MaterialSuplierId '{updateDto.MaterialSuplierId}' already exists.");
            }

            partNumberStructure.PartNumberLogisticsId = updateDto.PartNumberLogisticId;
            partNumberStructure.ProductionStationId = updateDto.ProductionStationId;
            partNumberStructure.MaterialSuplierId = updateDto.MaterialSuplierId;
            partNumberStructure.PartNumberName = updateDto.PartNumberName;
            partNumberStructure.PartNumberDescription = updateDto.PartNumberDescription;
            partNumberStructure.Active = updateDto.Active;
            partNumberStructure.UpdateDate = DateTime.UtcNow;
            partNumberStructure.UpdateBy = updateDto.UpdateBy;

            await _contextProductionControl.SaveChangesAsync();

            return await MapToDto(partNumberStructure);
        }

        private async Task<PartNumberStructureResponseDto> MapToDto(PartNumberStructure partNumberStructure)
        {
            var materialSupplier = await _materialSupplierService.GetById(partNumberStructure.MaterialSuplierId);
            var partNumberLogistic = await _partNumberLogisticsService.GetById(partNumberStructure.PartNumberLogisticsId);
            var productionStation = await _productionStationService.GetById(partNumberStructure.ProductionStationId);

            return new PartNumberStructureResponseDto
            {
                Id = partNumberStructure.Id,
                Active = partNumberStructure.Active,
                CreateDate = partNumberStructure.CreateDate,
                CreateBy = partNumberStructure.CreateBy,
                ProductionStationId = partNumberStructure.ProductionStationId,
                ProductionStation = productionStation,
                PartNumberDescription = partNumberStructure.PartNumberDescription,
                PartNumberLogisticId = partNumberStructure.PartNumberLogisticsId,
                PartNumberLogistic = partNumberLogistic,
                MaterialSupplierDescription = materialSupplier?.MaterialSupplierDescription ?? "N/A",
            };
        }
    }
}
