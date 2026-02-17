using Entity.Interfaces;
using Entity.ModelDtos.ProductionControl;
using Entity.Models.ProductionControl;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicDomain.ModelServices.ProductionControl
{
    public class MaterialSupplierService
    {
        private readonly ProductionControlContext _context;
        private readonly string _user = "System"; // Consistent with existing services

        public MaterialSupplierService(ProductionControlContext context)
        {
            _context = context;
        }

        public async Task<MaterialSupplierDto> Create(MaterialSupplierCreateDto createDto)
        {
            var newMaterialSupplier = new MaterialSupplier
            {
                Id = Guid.NewGuid(),
                Active = createDto.Active,
                CreateDate = DateTime.UtcNow,
                CreateBy = createDto.CreateBy,
                UpdateDate = DateTime.UtcNow,
                UpdateBy = _user,
                MaterialSupplierDescription = createDto.MaterialSupplierDescription
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
            materialSupplier.UpdateDate = DateTime.UtcNow;
            materialSupplier.UpdateBy = _user;
            _context.Entry(materialSupplier).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<MaterialSupplierDto>> GetAlls()
        {
            var materialSuppliers = await _context.MaterialSuppliers.Where(ms => ms.Active).ToListAsync();
            return materialSuppliers.Select(MapToDto).ToList();
        }

        public async Task<MaterialSupplierDto> GetById(Guid id)
        {
            var materialSupplier = await _context.MaterialSuppliers.FirstOrDefaultAsync(ms => ms.Id == id && ms.Active);
            return materialSupplier != null ? MapToDto(materialSupplier) : null;
        }

        public async Task<MaterialSupplierDto> Update(Guid id, MaterialSupplierUpdateDto updateDto)
        {
            var materialSupplier = await _context.MaterialSuppliers.FindAsync(id);
            if (materialSupplier == null)
            {
                throw new Exception("Material Supplier not found");
            }

            materialSupplier.Active = updateDto.Active;
            materialSupplier.MaterialSupplierDescription = updateDto.MaterialSupplierDescription;
            materialSupplier.UpdateDate = DateTime.UtcNow;
            materialSupplier.UpdateBy = updateDto.UpdateBy;

            _context.Entry(materialSupplier).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return MapToDto(materialSupplier);
        }

        private MaterialSupplierDto MapToDto(MaterialSupplier materialSupplier)
        {
            return new MaterialSupplierDto
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
