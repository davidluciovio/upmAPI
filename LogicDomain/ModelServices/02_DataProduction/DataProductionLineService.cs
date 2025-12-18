using Entity.Dtos.DataProduction.DataProductionLine;
using Entity.Interfaces;
using Entity.Models.DataProduction;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicDomain.DataProduction
{
    public class DataProductionLineService : IServiceCrud<ProductionLineDto, ProductionLineCreateDto, ProductionLineUpdateDto>
    {
        private readonly DataContext _dataContext;

        public DataProductionLineService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ProductionLineDto> Create(ProductionLineCreateDto createDto)
        {
            var productionLine = new DataProductionLine
            {
                Id = Guid.NewGuid(),
                Active = true,
                CreateDate = DateTime.UtcNow,
                CreateBy = createDto.CreateBy,
                LineDescription = createDto.LineDescription
            };

            _dataContext.ProductionLines.Add(productionLine);
            await _dataContext.SaveChangesAsync();

            return new ProductionLineDto
            {
                Id = productionLine.Id,
                Active = productionLine.Active,
                CreateDate = productionLine.CreateDate,
                CreateBy = productionLine.CreateBy,
                LineDescription = productionLine.LineDescription
            };
        }

        public async Task<List<ProductionLineDto>> GetAlls()
        {
            var productionLines = await _dataContext.ProductionLines.ToListAsync();

            return productionLines.Select(pl => new ProductionLineDto
            {
                Id = pl.Id,
                Active = pl.Active,
                CreateDate = pl.CreateDate,
                CreateBy = pl.CreateBy,
                LineDescription = pl.LineDescription
            }).ToList();
        }

        public async Task<ProductionLineDto?> GetById(Guid id)
        {
            var productionLine = await _dataContext.ProductionLines.FindAsync(id);
            if (productionLine == null)
            {
                throw new KeyNotFoundException("ProductionLine not found");
            }

            return new ProductionLineDto
            {
                Id = productionLine.Id,
                Active = productionLine.Active,
                CreateDate = productionLine.CreateDate,
                CreateBy = productionLine.CreateBy,
                LineDescription = productionLine.LineDescription
            };
        }

        public async Task<ProductionLineDto> Update(Guid id, ProductionLineUpdateDto updateDto)
        {
            var productionLine = await _dataContext.ProductionLines.FindAsync(id);
            if (productionLine == null)
            {
                throw new KeyNotFoundException("ProductionLine not found");
            }

            productionLine.Active = updateDto.Active;
            productionLine.LineDescription = updateDto.LineDescription;
            // Note: DataProductionLine model does not have UpdateBy or UpdateDate properties.
            // These properties are present in the DTO for consistency with other services but will not be mapped to the model.

            _dataContext.ProductionLines.Update(productionLine);
            await _dataContext.SaveChangesAsync();

            return new ProductionLineDto
            {
                Id = productionLine.Id,
                Active = productionLine.Active,
                CreateDate = productionLine.CreateDate,
                CreateBy = productionLine.CreateBy,
                LineDescription = productionLine.LineDescription
            };
        }
    }
}
