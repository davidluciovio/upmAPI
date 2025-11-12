using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicDomain
{
    public class DataProductionPartNumberService
    {
        private readonly DataContext _dataContext;
        public DataProductionPartNumberService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // Assuming DataProductionPartNumberCreateDto has the required fields
        public async Task<Entity.Dtos.DataProductionPartNumberDto> CreateProductionPartNumber(Entity.Dtos.DataProductionPartNumberCreateDto dto)
        {
            var partNumber = new Entity.Models.DataProductionPartNumber
            {
                Id = Guid.NewGuid(),
                Active = true,
                CreateDate = DateTime.Now,
                CreateBy = dto.CreateBy,
                PartNumberName = dto.PartNumberName,
                PartNumberDescription = dto.PartNumberDescription,
                SNP = dto.SNP,
                ProductionModelId = dto.ProductionModelId,
                ProductionLocationId = dto.ProductionLocationId,
                ProductionAreaId = dto.ProductionAreaId
            };
            _dataContext.ProductionPartNumbers.Add(partNumber);
            await _dataContext.SaveChangesAsync();

            // Re-fetch with includes to build the correct DTO
            var createdPartNumber = await _dataContext.ProductionPartNumbers
                .Include(p => p.DataProductionModel)
                .Include(p => p.DataProductionLocation)
                .Include(p => p.DataProductionArea)
                .FirstOrDefaultAsync(p => p.Id == partNumber.Id);

            return new Entity.Dtos.DataProductionPartNumberDto
            {
                Id = createdPartNumber.Id,
                Active = createdPartNumber.Active,
                CreateBy = createdPartNumber.CreateBy,
                CreateDate = createdPartNumber.CreateDate,
                PartNumberName = createdPartNumber.PartNumberName,
                PartNumberDescription = createdPartNumber.PartNumberDescription,
                SNP = createdPartNumber.SNP,
                ProductionModel = createdPartNumber.DataProductionModel?.ModelDescription,
                ProductionLocation = createdPartNumber.DataProductionLocation?.LocationDescription,
                ProductionArea = createdPartNumber.DataProductionArea?.AreaDescription
            };
        }

        public async Task<List<Entity.Dtos.DataProductionPartNumberDto>> GetAllProductionPartNumbers()
        {
            var partNumbers = await _dataContext.ProductionPartNumbers
                .Include(p => p.DataProductionModel)
                .Include(p => p.DataProductionLocation)
                .Include(p => p.DataProductionArea)
                .ToListAsync();

            return partNumbers.Select(partNumber => new Entity.Dtos.DataProductionPartNumberDto
            {
                Id = partNumber.Id,
                Active = partNumber.Active,
                CreateBy = partNumber.CreateBy,
                CreateDate = partNumber.CreateDate,
                PartNumberName = partNumber.PartNumberName,
                PartNumberDescription = partNumber.PartNumberDescription,
                SNP = partNumber.SNP,
                ProductionModel = partNumber.DataProductionModel?.ModelDescription,
                ProductionLocation = partNumber.DataProductionLocation?.LocationDescription,
                ProductionArea = partNumber.DataProductionArea?.AreaDescription
            }).ToList();
        }

        public async Task<Entity.Dtos.DataProductionPartNumberDto?> GetProductionPartNumberById(Guid id)
        {
            var partNumber = await _dataContext.ProductionPartNumbers
                .Include(p => p.DataProductionModel)
                .Include(p => p.DataProductionLocation)
                .Include(p => p.DataProductionArea)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (partNumber == null)
            {
                return null;
            }
            return new Entity.Dtos.DataProductionPartNumberDto
            {
                Id = partNumber.Id,
                Active = partNumber.Active,
                CreateBy = partNumber.CreateBy,
                CreateDate = partNumber.CreateDate,
                PartNumberName = partNumber.PartNumberName,
                PartNumberDescription = partNumber.PartNumberDescription,
                SNP = partNumber.SNP,
                ProductionModel = partNumber.DataProductionModel?.ModelDescription,
                ProductionLocation = partNumber.DataProductionLocation?.LocationDescription,
                ProductionArea = partNumber.DataProductionArea?.AreaDescription
            };
        }

        public async Task<bool> DeactivateProductionPartNumber(Guid id)
        {
            var partNumber = await _dataContext.ProductionPartNumbers.FindAsync(id);
            if (partNumber == null)
            {
                return false;
            }
            partNumber.Active = false;
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateProductionPartNumber(Guid id)
        {
            var partNumber = await _dataContext.ProductionPartNumbers.FindAsync(id);
            if (partNumber == null)
            {
                return false;
            }
            partNumber.Active = true;
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<Entity.Dtos.DataProductionPartNumberDto> UpdateProductionPartNumber(Guid id, Entity.Dtos.DataProductionPartNumberDto dto)
        {
            var partNumber = await _dataContext.ProductionPartNumbers.FindAsync(id);
            if (partNumber == null)
            {
                throw new KeyNotFoundException($"ProductionPartNumber with Id '{id}' not found.");
            }

            partNumber.Active = dto.Active;
            partNumber.PartNumberName = dto.PartNumberName;
            partNumber.PartNumberDescription = dto.PartNumberDescription;
            partNumber.SNP = dto.SNP;

            await _dataContext.SaveChangesAsync();

            var updatedPartNumber = await _dataContext.ProductionPartNumbers
                .Include(p => p.DataProductionModel)
                .Include(p => p.DataProductionLocation)
                .Include(p => p.DataProductionArea)
                .FirstOrDefaultAsync(p => p.Id == id);

            return new Entity.Dtos.DataProductionPartNumberDto
            {
                Id = updatedPartNumber.Id,
                Active = updatedPartNumber.Active,
                CreateBy = updatedPartNumber.CreateBy,
                CreateDate = updatedPartNumber.CreateDate,
                PartNumberName = updatedPartNumber.PartNumberName,
                PartNumberDescription = updatedPartNumber.PartNumberDescription,
                SNP = updatedPartNumber.SNP,
                ProductionModel = updatedPartNumber.DataProductionModel?.ModelDescription,
                ProductionLocation = updatedPartNumber.DataProductionLocation?.LocationDescription,
                ProductionArea = updatedPartNumber.DataProductionArea?.AreaDescription
            };
        }
    }
}
