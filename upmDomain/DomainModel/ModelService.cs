using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upmData.Context;
using upmDomain.Interfaces;

namespace upmDomain.DomainModel
{
    public class ModelService : IService<ModelDto>
    {
        private readonly UpmwebContext _context;
        public ModelService(UpmwebContext context) 
        {
            _context = context;
        }

        public async Task<List<ModelDto>> GetAllAsync(List<Guid> linesIds)
        {
            return await _context.PartNumberConfigurations
                .Where(pc => linesIds.Contains(pc.LineId))
                .Select(pc => new ModelDto
                {
                    ModelDescription = pc.Model.Name,
                    ModelId = pc.ModelId
                })
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<ModelDto>> GetAllAsync(List<Guid> linesIds, List<Guid> liderIds)
        {
            return await _context.LiderConfigurations
                .Where(lc => liderIds.Contains(lc.UserId) && linesIds.Contains(lc.PartNumberConfiguration.LineId))
                .Select(lc => new ModelDto
                {
                    ModelDescription = lc.PartNumberConfiguration.Model.Name,
                    ModelId = lc.PartNumberConfiguration.ModelId
                })
                .Distinct()
                .ToListAsync();
        }

        public Task<List<ModelDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
