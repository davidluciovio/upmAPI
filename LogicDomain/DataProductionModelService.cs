using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicDomain
{
    public class DataProductionModelService
    {
        private readonly DataContext _dataContext;
        public DataProductionModelService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Entity.Dtos.DataProductionModelDto> CreateProductionModel(Entity.Dtos.DataProductionModelCreateDto modelDto)
        {
            var model = new Entity.Models.DataProductionModel
            {
                Active = true,
                CreateBy = modelDto.CreateBy,
                CreateDate = DateTime.Now,
                Id = Guid.NewGuid(),
                ModelDescription = modelDto.ModelDescription
            };
            _dataContext.ProductionModels.Add(model);
            await _dataContext.SaveChangesAsync();
            return new Entity.Dtos.DataProductionModelDto
            {
                Id = model.Id,
                Active = model.Active,
                CreateBy = model.CreateBy,
                CreateDate = model.CreateDate,
                ModelDescription = model.ModelDescription
            };
        }

        public async Task<List<Entity.Dtos.DataProductionModelDto>> GetAllProductionModels()
        {
            var models = await _dataContext.ProductionModels.ToListAsync();
            return models.Select(model => new Entity.Dtos.DataProductionModelDto
            {
                Id = model.Id,
                Active = model.Active,
                CreateBy = model.CreateBy,
                CreateDate = model.CreateDate,
                ModelDescription = model.ModelDescription
            }).ToList();
        }

        public async Task<Entity.Dtos.DataProductionModelDto?> GetProductionModelById(Guid id)
        {
            var model = await _dataContext.ProductionModels.FindAsync(id);
            if (model == null)
            {
                return null;
            }
            return new Entity.Dtos.DataProductionModelDto
            {
                Id = model.Id,
                Active = model.Active,
                CreateBy = model.CreateBy,
                CreateDate = model.CreateDate,
                ModelDescription = model.ModelDescription
            };
        }

        public async Task<bool> DeactivateProductionModel(Guid id)
        {
            var model = await _dataContext.ProductionModels.FindAsync(id);
            if (model == null)
            {
                return false;
            }
            model.Active = false;
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateProductionModel(Guid id)
        {
            var model = await _dataContext.ProductionModels.FindAsync(id);
            if (model == null)
            {
                return false;
            }
            model.Active = true;
            await _dataContext.SaveChangesAsync();
            return true;
        }   

        public async Task<Entity.Dtos.DataProductionModelDto> UpdateProductionModel(Guid id, Entity.Dtos.DataProductionModelDto modelDto)
        {
            var model = await _dataContext.ProductionModels.FindAsync(id);
            if (model == null)
            {
                throw new KeyNotFoundException($"ProductionModel with Id '{id}' not found.");
            }

            model.Active = modelDto.Active;
            model.ModelDescription = modelDto.ModelDescription;
            model.CreateBy = modelDto.CreateBy;
            model.CreateDate = DateTime.Now;

            await _dataContext.SaveChangesAsync();

            return new Entity.Dtos.DataProductionModelDto
            {
                Id = model.Id,
                Active = model.Active,
                CreateBy = model.CreateBy,
                CreateDate = model.CreateDate,
                ModelDescription = model.ModelDescription
            };
        }
    }
}
