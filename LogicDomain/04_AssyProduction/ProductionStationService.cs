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

namespace LogicDomain._04_AssyProduction
{
    public class ProductionStationService : IServiceCrud<ProductionStationDto, ProductionStationCreateDto, ProductionStationUpdateDto>
    {
        private readonly AssyProductionContext _assyProductionContext;
        private readonly DataContext _dataContext;

        public ProductionStationService(AssyProductionContext assyProductionContext, DataContext dataContext)
        {
            _assyProductionContext = assyProductionContext;
            _dataContext = dataContext;
        }

        public async Task<ProductionStationDto> Create(ProductionStationCreateDto createDto)
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

            return new ProductionStationDto
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

        public async Task<List<ProductionStationDto>> GetAlls()
        {
            var stations = await _assyProductionContext.ProductionStations.ToListAsync();

            return stations.Select(s => new ProductionStationDto
            {
                Active = s.Active,
                CreateBy = s.CreateBy,
                CreateDate = s.CreateDate,
                UpdateBy = s.UpdateBy,
                UpdateDate = s.UpdateDate,
                Id = s.Id,
                PartNumber = _dataContext.ProductionPartNumbers.Find(s.PartNumberId)?.PartNumberName ?? "N/A",
                Line = _dataContext.ProductionLines.Find(s.LineId)?.LineDescription ?? "N/A",
                Model = _dataContext.ProductionModels.Find(s.ModelId)?.ModelDescription ?? "N/A",
                NetoTime = s.NetoTime,
                ObjetiveTime = s.ObjetiveTime,
                OperatorQuantity = s.OperatorQuantity,
                PartNumberQuantity = s.PartNumberQuantity
            }).ToList();
        }

        public async Task<ProductionStationDto?> GetById(Guid id)
        {
            var station = await _assyProductionContext.ProductionStations.FindAsync(id);
            if (station == null)
            {
                throw new KeyNotFoundException("ProductionStation not found");
            }

            return new ProductionStationDto
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

        public async Task<ProductionStationDto> Update(Guid id, ProductionStationUpdateDto updateDto)
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

            return new ProductionStationDto
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
