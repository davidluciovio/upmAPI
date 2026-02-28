using Entity.Dtos.AssyProduction;
using Entity.Interfaces;
using Entity.Models.AssyProduction;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace LogicDomain.AssyProduction
{
    public class ProductionStationService : IServiceCrud<ProductionStationResponseDto, ProductionStationCreateDto, ProductionStationUpdateDto>
    {
        private readonly AssyProductionContext _assyProductionContext;
        private readonly DataContext _dataContext;

        public ProductionStationService(AssyProductionContext assyProductionContext, DataContext dataContext)
        {
            _assyProductionContext = assyProductionContext;
            _dataContext = dataContext;
        }

        public async Task<ProductionStationResponseDto> Create(ProductionStationCreateDto createDto)
        {
            var station = new ProductionStation
            {
                CreateBy = createDto.CreateBy,
                CreateDate = DateTime.UtcNow,
                UpdateBy = createDto.UpdateBy,
                UpdateDate = DateTime.UtcNow,
                PartNumberId = createDto.PartNumberId,
                LineId = createDto.LineId,
                ModelId = createDto.ModelId,
                Active = true,
                NetoTime = createDto.NetoTime,
                ObjetiveTime = createDto.ObjetiveTime,
                OperatorQuantity = createDto.OperatorQuantity,
                PartNumberQuantity = createDto.PartNumberQuantity
            };
            _assyProductionContext.ProductionStations.Add(station);
            await _assyProductionContext.SaveChangesAsync();

            return new ProductionStationResponseDto
            {
                Active = station.Active,
                CreateBy = station.CreateBy,
                CreateDate = station.CreateDate,
                UpdateBy = station.UpdateBy,
                UpdateDate = station.UpdateDate,
                Id = station.Id,
                PartNumber = _dataContext.ProductionPartNumbers.Find(station.PartNumberId)?.PartNumberName ?? "N/A",
                Line = _dataContext.ProductionLines.Find(station.LineId)?.LineDescription ?? "N/A",
                Model = _dataContext.ProductionModels.Find(station.ModelId)?.ModelDescription ?? "N/A",
                NetoTime = station.NetoTime,
                ObjetiveTime = station.ObjetiveTime,
                OperatorQuantity = station.OperatorQuantity,
                PartNumberQuantity = station.PartNumberQuantity
            };
        }

        public async Task<List<ProductionStationResponseDto>> GetAlls()
        {
            // 1. Traer todas las estaciones (Usamos AsNoTracking para mayor velocidad de lectura)
            var stations = await _assyProductionContext.ProductionStations
                                                       .AsNoTracking()
                                                       .ToListAsync();

            // 2. Obtener los IDs únicos necesarios para no traer toda la base de datos secundaria
            var partIds = stations.Select(s => s.PartNumberId).Where(id => id != null).Distinct().ToList();
            var lineIds = stations.Select(s => s.LineId).Where(id => id != null).Distinct().ToList();
            var modelIds = stations.Select(s => s.ModelId).Where(id => id != null).Distinct().ToList();

            // 3. Traer la información de referencia en bloque desde el segundo contexto (_dataContext)
            //    y convertirla a Diccionarios para búsqueda instantánea.
            var partsDict = await _dataContext.ProductionPartNumbers
                .Where(p => partIds.Contains(p.Id))
                .ToDictionaryAsync(k => k.Id, v => v.PartNumberName);

            var linesDict = await _dataContext.ProductionLines
                .Where(l => lineIds.Contains(l.Id))
                .ToDictionaryAsync(k => k.Id, v => v.LineDescription);

            var modelsDict = await _dataContext.ProductionModels
                .Where(m => modelIds.Contains(m.Id))
                .ToDictionaryAsync(k => k.Id, v => v.ModelDescription);

            // 4. Mapear los datos en memoria (esto es extremadamente rápido)
            var result = stations.Select(s => new ProductionStationResponseDto
            {
                Active = s.Active,
                CreateBy = s.CreateBy,
                CreateDate = s.CreateDate,
                UpdateBy = s.UpdateBy,
                UpdateDate = s.UpdateDate,
                Id = s.Id,

                // Buscamos en el diccionario en lugar de ir a la BD
                PartNumber = (s.PartNumberId != null && partsDict.TryGetValue(s.PartNumberId, out var partName))
                             ? partName : "N/A",

                Line = (s.LineId != null && linesDict.TryGetValue(s.LineId, out var lineName))
                       ? lineName : "N/A",

                Model = (s.ModelId != null && modelsDict.TryGetValue(s.ModelId, out var modelName))
                        ? modelName : "N/A",

                NetoTime = s.NetoTime,
                ObjetiveTime = s.ObjetiveTime,
                OperatorQuantity = s.OperatorQuantity,
                PartNumberQuantity = s.PartNumberQuantity
            }).ToList();

            return result;
        }

        public async Task<ProductionStationResponseDto?> GetById(Guid id)
        {
            var station = await _assyProductionContext.ProductionStations.FindAsync(id);
            if (station == null)
            {
                throw new KeyNotFoundException("ProductionStation not found");
            }

            return new ProductionStationResponseDto
            {
                Active = station.Active,
                CreateBy = station.CreateBy,
                CreateDate = station.CreateDate,
                UpdateBy = station.UpdateBy,
                UpdateDate = station.UpdateDate,
                Id = station.Id,
                PartNumber = _dataContext.ProductionPartNumbers.Find(station.PartNumberId)?.PartNumberName ?? "N/A",
                Line = _dataContext.ProductionLines.Find(station.LineId)?.LineDescription ?? "N/A",
                Model = _dataContext.ProductionModels.Find(station.ModelId)?.ModelDescription ?? "N/A",
                NetoTime = station.NetoTime,
                ObjetiveTime = station.ObjetiveTime,
                OperatorQuantity = station.OperatorQuantity,
                PartNumberQuantity = station.PartNumberQuantity
            };
        }

        public async Task<ProductionStationResponseDto> Update(Guid id, ProductionStationUpdateDto updateDto)
        {
            var station = await _assyProductionContext.ProductionStations.FindAsync(id);
            if (station == null) throw new KeyNotFoundException("ProductionStation not found");

            var partNumber = await _dataContext.ProductionPartNumbers.FindAsync(updateDto.PartNumberId);
            if (partNumber == null) throw new KeyNotFoundException("PartNumber not found");

            var line = await _dataContext.ProductionLines.FindAsync(updateDto.LineId);
            if (line == null) throw new KeyNotFoundException("Line not found");

            var model = await _dataContext.ProductionModels.FindAsync(updateDto.ModelId);
            if (model == null) throw new KeyNotFoundException("Model not found");

            station.Active = updateDto.Active;
            station.PartNumberId = partNumber.Id;
            station.LineId = line.Id;
            station.ModelId = updateDto.ModelId;
            station.UpdateBy = updateDto.UpdateBy;
            station.UpdateDate = DateTime.UtcNow;
            station.ObjetiveTime = updateDto.ObjetiveTime;
            station.NetoTime = updateDto.NetoTime;
            station.OperatorQuantity = updateDto.OperatorQuantity;
            station.PartNumberQuantity = updateDto.PartNumberQuantity;

            _assyProductionContext.ProductionStations.Update(station);
            await _assyProductionContext.SaveChangesAsync();

            return new ProductionStationResponseDto
            {
                Active = station.Active,
                CreateBy = station.CreateBy,
                CreateDate = station.CreateDate,
                UpdateBy = station.UpdateBy,
                UpdateDate = station.UpdateDate,
                Id = station.Id,
                PartNumber = partNumber.PartNumberName,
                Line = line.LineDescription,
                Model = model.ModelDescription,
                NetoTime = station.NetoTime,
                ObjetiveTime = station.ObjetiveTime,
                OperatorQuantity = station.OperatorQuantity,
                PartNumberQuantity = station.PartNumberQuantity
            };
        }
    }
}
