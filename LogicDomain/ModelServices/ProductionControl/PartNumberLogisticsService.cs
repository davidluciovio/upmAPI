using Entity.Dtos.ProductionControl;
using Entity.Interfaces;
using Entity.Models.ProductionControl;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicDomain.ProductionControl
{
    public class PartNumberLogisticsService : IServiceCrud<PartNumberLogisticsResponseDto, PartNumberLogisticsCreateDto, PartNumberLogisticsUpdateDto>
    {
        private readonly ProductionControlContext _productionControlContext;
        private readonly DataContext _dataContext;

        public PartNumberLogisticsService(ProductionControlContext productionControlContext, DataContext dataContext) 
        {
            _productionControlContext = productionControlContext;
            _dataContext = dataContext;
        }

        public async Task<PartNumberLogisticsResponseDto> Create(PartNumberLogisticsCreateDto createDto)
        {
            var partNumberLogistic = new PartNumberLogistics
            {
                PartNumberId = createDto.PartNumberId,
                AreaId = createDto.AreaId,
                LocationId = createDto.LocationId,
                SNP = createDto.SNP,
                Active = true,
                CreateBy = createDto.CreateBy,
                CreateDate = DateTime.UtcNow,
                UpdateBy = createDto.UpdateBy,
                UpdateDate = DateTime.UtcNow
            };
            _productionControlContext.PartNumberLogistics.Add(partNumberLogistic);
            await _productionControlContext.SaveChangesAsync();

            return new PartNumberLogisticsResponseDto
            {
                Id = partNumberLogistic.Id,
                PartNumber = partNumberLogistic.PartNumberId.ToString(),
                Area = partNumberLogistic.AreaId.ToString(),
                Location = partNumberLogistic.LocationId.ToString(),
                SNP = partNumberLogistic.SNP,
                Active = partNumberLogistic.Active,
                CreateBy = partNumberLogistic.CreateBy,
                CreateDate = partNumberLogistic.CreateDate,
                UpdateBy = partNumberLogistic.UpdateBy,
                UpdateDate = partNumberLogistic.UpdateDate
            };
        }


        public async Task<List<PartNumberLogisticsResponseDto>> GetAlls()
        {
            // 1. Traer la lista principal (Rápido, sin rastreo de cambios)
            var logs = await _productionControlContext.PartNumberLogistics
                                                      .AsNoTracking()
                                                      .ToListAsync();

            // 2. Extraer los IDs únicos necesarios para hacer las consultas en bloque
            //    (Asumo que los IDs pueden ser null, por eso el check de null)
            var partIds = logs.Select(x => x.PartNumberId).Distinct().ToList();
            var areaIds = logs.Select(x => x.AreaId).Distinct().ToList();
            var locationIds = logs.Select(x => x.LocationId).Distinct().ToList();

            // 3. Traer los datos de referencia del segundo contexto y convertirlos a Diccionarios
            //    Usamos 'Contains' para traer solo lo necesario en una sola consulta por tabla.
            var partsDict = await _dataContext.ProductionPartNumbers
                .Where(p => partIds.Contains(p.Id))
                .ToDictionaryAsync(k => k.Id, v => v.PartNumberName);

            var areasDict = await _dataContext.ProductionAreas
                .Where(a => areaIds.Contains(a.Id))
                .ToDictionaryAsync(k => k.Id, v => v.AreaDescription);

            var locationsDict = await _dataContext.ProductionLocations
                .Where(l => locationIds.Contains(l.Id))
                .ToDictionaryAsync(k => k.Id, v => v.LocationDescription);

            // 4. Mapear los resultados en memoria (O(1) de velocidad gracias a los diccionarios)
            var result = logs.Select(pna => new PartNumberLogisticsResponseDto
            {
                Id = pna.Id,

                // Lógica: Intenta obtener el nombre del diccionario. 
                // Si no existe (o el ID es null), devuelve el ID como string (tal como tu código original).
                PartNumber = (pna.PartNumberId != null && partsDict.TryGetValue(pna.PartNumberId, out var partName))
                             ? partName
                             : pna.PartNumberId.ToString(),

                Area = (pna.AreaId != null && areasDict.TryGetValue(pna.AreaId, out var areaName))
                       ? areaName
                       : pna.AreaId.ToString(),

                Location = (pna.LocationId != null && locationsDict.TryGetValue(pna.LocationId, out var locName))
                           ? locName
                           : pna.LocationId.ToString(),

                SNP = pna.SNP,
                Active = pna.Active,
                CreateBy = pna.CreateBy,
                CreateDate = pna.CreateDate,
                UpdateBy = pna.UpdateBy,
                UpdateDate = pna.UpdateDate
            }).ToList();

            return result;
        }

        public async Task<PartNumberLogisticsResponseDto?> GetById(Guid id)
        {
            var partNumberLogistic = await _productionControlContext.PartNumberLogistics.FindAsync(id);
            if (partNumberLogistic == null)
            {
                throw new KeyNotFoundException("partNumberLogistic not found");
            }
            return new PartNumberLogisticsResponseDto
            {
                Id = partNumberLogistic.Id,
                PartNumber = _dataContext.ProductionPartNumbers
                    .Where(pn => pn.Id == partNumberLogistic.PartNumberId)
                    .Select(pn => pn.PartNumberName)
                    .FirstOrDefault() ?? partNumberLogistic.PartNumberId.ToString(),
                Area = _dataContext.ProductionAreas
                    .Where(a => a.Id == partNumberLogistic.AreaId)
                    .Select(a => a.AreaDescription)
                    .FirstOrDefault() ?? partNumberLogistic.AreaId.ToString(),
                Location = _dataContext.ProductionLocations
                    .Where(l => l.Id == partNumberLogistic.LocationId)
                    .Select(l => l.LocationDescription)
                    .FirstOrDefault() ?? partNumberLogistic.LocationId.ToString(),
                SNP = partNumberLogistic.SNP,
                Active = partNumberLogistic.Active,
                CreateBy = partNumberLogistic.CreateBy,
                CreateDate = partNumberLogistic.CreateDate,
                UpdateBy = partNumberLogistic.UpdateBy,
                UpdateDate = partNumberLogistic.UpdateDate
            };
        }

        public async Task<PartNumberLogisticsResponseDto> Update(Guid id, PartNumberLogisticsUpdateDto updateDto)
        {
            var partNumberLogistic = _productionControlContext.PartNumberLogistics.Find(id);
            if (partNumberLogistic == null) throw new KeyNotFoundException("partNumberLogistic not found");

            var parNumber = _dataContext.ProductionPartNumbers
                .Find(updateDto.PartNumberId);
            if (parNumber == null) throw new KeyNotFoundException("PartNumber not found");

            var area = _dataContext.ProductionAreas
                .Find(updateDto.AreaId);
            if (area == null) throw new KeyNotFoundException("Area not found");

            var location = _dataContext.ProductionLocations
                .Find(updateDto.LocationId);
            if (location == null) throw new KeyNotFoundException("PartNumber not found");

            partNumberLogistic.Active = updateDto.Active;
            partNumberLogistic.PartNumberId = parNumber.Id;
            partNumberLogistic.AreaId = area.Id;
            partNumberLogistic.LocationId = updateDto.LocationId;
            partNumberLogistic.SNP = updateDto.SNP;
            partNumberLogistic.UpdateBy = updateDto.UpdateBy;
            partNumberLogistic.UpdateDate = DateTime.UtcNow;

            _productionControlContext.PartNumberLogistics.Update(partNumberLogistic);
            await _productionControlContext.SaveChangesAsync();

            return new PartNumberLogisticsResponseDto
            {
                Id = partNumberLogistic.Id,
                PartNumber = parNumber.PartNumberName,
                Area = area.AreaDescription,
                Location = location.LocationDescription,
                SNP = partNumberLogistic.SNP,
                Active = partNumberLogistic.Active,
                CreateBy = partNumberLogistic.CreateBy,
                CreateDate = partNumberLogistic.CreateDate,
                UpdateBy = partNumberLogistic.UpdateBy,
                UpdateDate = partNumberLogistic.UpdateDate
            };
        }
    }
}
