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
                PartNumberId = createDto.PartNumberId,
                LineId = createDto.LineId,
                Active = true,
                CreateBy = createDto.CreateBy,
                CreateDate = DateTime.UtcNow,
                UpdateBy = createDto.UpdateBy,
                UpdateDate = DateTime.UtcNow
            };
            _assyProductionContext.ProductionStations.Add(station);
            await _assyProductionContext.SaveChangesAsync();

            return new ProductionStationDto
            {
                Id = station.Id,
                PartNumber = _dataContext.ProductionPartNumbers.Find(station.PartNumberId)?.PartNumberName ?? "N/A",
                Line = _dataContext.ProductionLines.Find(station.LineId)?.LineDescription ?? "N/A",
                Active = station.Active,
                CreateBy = station.CreateBy,
                CreateDate = station.CreateDate,
                UpdateBy = station.UpdateBy,
                UpdateDate = station.UpdateDate
            };
        }

        public async Task<List<ProductionStationDto>> GetAlls()
        {
            var stations = await _assyProductionContext.ProductionStations.ToListAsync();

            return stations.Select(s => new ProductionStationDto
            {
                Id = s.Id,
                PartNumber = _dataContext.ProductionPartNumbers.Find(s.PartNumberId)?.PartNumberName ?? "N/A",
                Line = _dataContext.ProductionLines.Find(s.LineId)?.LineDescription ?? "N/A",
                Active = s.Active,
                CreateBy = s.CreateBy,
                CreateDate = s.CreateDate,
                UpdateBy = s.UpdateBy,
                UpdateDate = s.UpdateDate
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
                Id = station.Id,
                PartNumber = _dataContext.ProductionPartNumbers.Find(station.PartNumberId)?.PartNumberName ?? "N/A",
                Line = _dataContext.ProductionLines.Find(station.LineId)?.LineDescription ?? "N/A",
                Active = station.Active,
                CreateBy = station.CreateBy,
                CreateDate = station.CreateDate,
                UpdateBy = station.UpdateBy,
                UpdateDate = station.UpdateDate
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

            station.Active = updateDto.Active;
            station.PartNumberId = partNumber.Id;
            station.LineId = line.Id;
            station.UpdateBy = updateDto.UpdateBy;
            station.UpdateDate = DateTime.UtcNow;

            _assyProductionContext.ProductionStations.Update(station);
            await _assyProductionContext.SaveChangesAsync();

            return new ProductionStationDto
            {
                Id = station.Id,
                PartNumber = partNumber.PartNumberName,
                Line = line.LineDescription,
                Active = station.Active,
                CreateBy = station.CreateBy,
                CreateDate = station.CreateDate,
                UpdateBy = station.UpdateBy,
                UpdateDate = station.UpdateDate
            };
        }
    }
}
