using Entity.Dtos.DataProduction;
using Entity.Models.DataProduction;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicDomain.DataProduction
{
    public class DataProductionPartNumberService
    {
        private readonly DataContext _dataContext;
        public DataProductionPartNumberService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // Assuming DataProductionPartNumberCreateDto has the required fields
        public async Task<DataProductionPartNumberDto> CreateProductionPartNumber(DataProductionPartNumberCreateDto dto)
        {
            var partNumber = new DataProductionPartNumber
            {
                Id = Guid.NewGuid(),
                Active = true,
                CreateDate = DateTime.Now,
                CreateBy = dto.CreateBy,
                PartNumberName = dto.PartNumberName,
                PartNumberDescription = dto.PartNumberDescription,
              
            };
            _dataContext.ProductionPartNumbers.Add(partNumber);
            await _dataContext.SaveChangesAsync();

            // Re-fetch with includes to build the correct DTO
            var createdPartNumber = await _dataContext.ProductionPartNumbers
                .FirstOrDefaultAsync(p => p.Id == partNumber.Id);

            return new DataProductionPartNumberDto
            {
                Id = createdPartNumber!.Id,
                Active = createdPartNumber.Active,
                CreateBy = createdPartNumber.CreateBy,
                CreateDate = createdPartNumber.CreateDate,
                PartNumberName = createdPartNumber.PartNumberName,
                PartNumberDescription = createdPartNumber.PartNumberDescription
            };
        }

        public async Task<List<DataProductionPartNumberDto>> GetAllProductionPartNumbers()
        {
            var partNumbers = await _dataContext.ProductionPartNumbers
                .ToListAsync();

            return partNumbers.Select(partNumber => new DataProductionPartNumberDto
            {
                Id = partNumber.Id,
                Active = partNumber.Active,
                CreateBy = partNumber.CreateBy,
                CreateDate = partNumber.CreateDate,
                PartNumberName = partNumber.PartNumberName,
                PartNumberDescription = partNumber.PartNumberDescription,
            }).ToList();
        }

        public async Task<DataProductionPartNumberDto?> GetProductionPartNumberById(Guid id)
        {
            var partNumber = await _dataContext.ProductionPartNumbers
                .FirstOrDefaultAsync(p => p.Id == id);

            if (partNumber == null)
            {
                return null;
            }
            return new DataProductionPartNumberDto
            {
                Id = partNumber.Id,
                Active = partNumber.Active,
                CreateBy = partNumber.CreateBy,
                CreateDate = partNumber.CreateDate,
                PartNumberName = partNumber.PartNumberName,
                PartNumberDescription = partNumber.PartNumberDescription
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

        public async Task<DataProductionPartNumberDto> UpdateProductionPartNumber(Guid id, DataProductionPartNumberDto dto)
        {
            var partNumber = await _dataContext.ProductionPartNumbers.FindAsync(id);
            if (partNumber == null)
            {
                throw new KeyNotFoundException($"ProductionPartNumber with Id '{id}' not found.");
            }

            partNumber.Active = dto.Active;
            partNumber.PartNumberName = dto.PartNumberName;
            partNumber.PartNumberDescription = dto.PartNumberDescription;

            await _dataContext.SaveChangesAsync();

            var updatedPartNumber = await _dataContext.ProductionPartNumbers
                .FirstOrDefaultAsync(p => p.Id == id);

            return new DataProductionPartNumberDto
            {
                Id = updatedPartNumber!.Id,
                Active = updatedPartNumber.Active,
                CreateBy = updatedPartNumber.CreateBy,
                CreateDate = updatedPartNumber.CreateDate,
                PartNumberName = updatedPartNumber.PartNumberName,
                PartNumberDescription = updatedPartNumber.PartNumberDescription
            };
        }
    }
}
