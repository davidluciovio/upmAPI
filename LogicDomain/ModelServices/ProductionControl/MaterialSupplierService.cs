using Entity.Dtos.ModelDtos.ProductionControl.MaterialSupplier;
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
    public class MaterialSupplierService : IService<MaterialSupplierResponseDto, MaterialSupplierRequestDto>
    {
        private readonly ProductionControlContext _context;

        public MaterialSupplierService(ProductionControlContext context)
        {
            _context = context;
        }

        public async Task<MaterialSupplierResponseDto> Create(MaterialSupplierRequestDto createDto)
        {
            if (await _context.MaterialSuppliers.AnyAsync(ms => ms.MaterialSupplierDescription == createDto.MaterialSupplierDescription))
            {
                throw new InvalidOperationException($"MaterialSupplier with description '{createDto.MaterialSupplierDescription}' already exists.");
            }

            var newMaterialSupplier = new MaterialSupplier
            {
                MaterialSupplierDescription = createDto.MaterialSupplierDescription.ToUpper(),
                CreateBy = createDto.CreateBy,
                Active = true,
                CreateDate = DateTime.UtcNow
            };

            _context.MaterialSuppliers.Add(newMaterialSupplier);
            await _context.SaveChangesAsync();

            return MapToDto(newMaterialSupplier);
        }

        public async Task<bool> Delete(Guid id)
        {
            var materialSupplier = await _context.MaterialSuppliers.FindAsync(id);
            if (materialSupplier == null)
            {
                return false;
            }

            materialSupplier.Active = false; // Soft delete
            
            _context.Entry(materialSupplier).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<MaterialSupplierResponseDto>> GetAlls()
        {
            return await _context.MaterialSuppliers
                .Where(ms => ms.Active)
                .Select(ms => new MaterialSupplierResponseDto
                {
                    Id = ms.Id,
                    Active = ms.Active,
                    CreateDate = ms.CreateDate,
                    CreateBy = ms.CreateBy,
                    UpdateDate = ms.UpdateDate,
                    UpdateBy = ms.UpdateBy,
                    MaterialSupplierDescription = ms.MaterialSupplierDescription
                })
                .ToListAsync();
        }

        public async Task<MaterialSupplierResponseDto?> GetById(Guid id)
        {
            var materialSupplier = await _context.MaterialSuppliers.FirstOrDefaultAsync(ms => ms.Id == id && ms.Active);
            return materialSupplier != null ? MapToDto(materialSupplier) : null;
        }

        public async Task<MaterialSupplierResponseDto> Update(Guid id, MaterialSupplierRequestDto updateDto)
        {
            var materialSupplier = await _context.MaterialSuppliers.FindAsync(id);
            if (materialSupplier == null)
            {
                throw new KeyNotFoundException($"Material Supplier with ID '{id}' not found.");
            }
            if (await _context.MaterialSuppliers.AnyAsync(m => m.Id != id && m.MaterialSupplierDescription == updateDto.MaterialSupplierDescription))
            {
                throw new InvalidOperationException($"Another MaterialSupplier with description '{updateDto.MaterialSupplierDescription}' already exists.");
            }


            materialSupplier.MaterialSupplierDescription = updateDto.MaterialSupplierDescription;
            materialSupplier.UpdateDate = DateTime.UtcNow;
            materialSupplier.UpdateBy = updateDto.UpdateBy;
            materialSupplier.Active = updateDto.Active;

            await _context.SaveChangesAsync();

            return MapToDto(materialSupplier);
        }

        private MaterialSupplierResponseDto MapToDto(MaterialSupplier materialSupplier)
        {
            return new MaterialSupplierResponseDto
            {
                Id = materialSupplier.Id,
                Active = materialSupplier.Active,
                CreateDate = materialSupplier.CreateDate,
                CreateBy = materialSupplier.CreateBy,
                UpdateDate = materialSupplier.UpdateDate,
                UpdateBy = materialSupplier.UpdateBy,
                MaterialSupplierDescription = materialSupplier.MaterialSupplierDescription
            };
        }
    }
}
