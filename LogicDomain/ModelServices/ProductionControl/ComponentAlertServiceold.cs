using Entity.Dtos.ModelDtos.ProductionControl.ComponentAlert;
using Entity.Dtos.ProductionControl;
using Entity.Interfaces;
using Entity.Models.ProductionControl;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicDomain.ModelServices
{
    public class ComponentAlertServiceold : IServiceCrud<ComponentAlertResponseDto, ComponentAlertRequestDto, ComponentAlertRequestDto>
    {
        private readonly ProductionControlContext _context;
        private readonly DataContext _contextData;

        public ComponentAlertServiceold(ProductionControlContext context, DataContext contextData)
        {
            _context = context;
            _contextData = contextData;
        }

        public async Task<ComponentAlertResponseDto> Create(ComponentAlertRequestDto createDto)
        {
            var newAlert = new ComponentAlert
            {
                Active = true,
                CreateBy = createDto.CreateBy,
                CreateDate = DateTime.Now,
                UpdateBy = createDto.CreateBy,
                UpdateDate = DateTime.Now,
                PartNumberLogisticsId = createDto.PartNumberLogisticsId,
            };

            _context.ComponentAlerts.Add(newAlert);
            await _context.SaveChangesAsync();
            return (await GetById(newAlert.Id))!;
        }

        public async Task<List<ComponentAlertResponseDto>> GetAlls()
        {
            // PASO 1: Consultar solo el primer contexto
            var baseData = await (from ca in _context.ComponentAlerts
                                  join pnl in _context.PartNumberLogistics on ca.PartNumberLogisticsId equals pnl.Id
                                  select new { ca, pnl }).ToListAsync();

            if (!baseData.Any()) return new List<ComponentAlertResponseDto>();

            // PASO 2: Extraer IDs únicos para filtrar el segundo contexto
            var areaIds = baseData.Select(x => x.pnl.AreaId).Distinct().ToList();
            var locationIds = baseData.Select(x => x.pnl.LocationId).Distinct().ToList();
            var partIds = baseData.Select(x => x.pnl.PartNumberId).Distinct().ToList();

            // PASO 3: Consultar el segundo contexto solo por lo que necesitamos
            var areas = await _contextData.ProductionAreas.Where(a => areaIds.Contains(a.Id)).ToListAsync();
            var locations = await _contextData.ProductionLocations.Where(l => locationIds.Contains(l.Id)).ToListAsync();
            var parts = await _contextData.ProductionPartNumbers.Where(p => partIds.Contains(p.Id)).ToListAsync();

            // PASO 4: Unir todo en memoria (LINQ to Objects)
            return baseData.Select(x => new ComponentAlertResponseDto
            {
                Id = x.ca.Id,
                Active = x.ca.Active,
                CreateDate = x.ca.CreateDate,
                CreateBy = x.ca.CreateBy,
                UpdateDate = x.ca.UpdateDate,
                UpdateBy = x.ca.UpdateBy,
                CompleteBy = x.ca.CompleteBy,
                CompleteDate = x.ca.CompleteDate,
                ReceivedBy = x.ca.ReceivedBy,
                ReceivedDate = x.ca.ReceivedDate,
                CancelBy = x.ca.CancelBy,
                CancelDate = x.ca.CancelDate,
                CriticalBy = x.ca.CriticalBy,
                CriticalDate = x.ca.CriticalDate,
                PartNumberLogisticsId = x.ca.PartNumberLogisticsId,
                StatusDescrition = GetStatusDescription(x.ca), // Aquí sí funciona porque es memoria
                PartNumberLogisticsResponseDto = new PartNumberLogisticsResponseDto
                {
                    Id = x.pnl.Id,
                    Active = x.pnl.Active,
                    CreateBy = x.pnl.CreateBy,
                    CreateDate = x.pnl.CreateDate,
                    UpdateBy = x.pnl.UpdateBy,
                    UpdateDate = x.pnl.UpdateDate,
                    // Buscamos las descripciones en las listas obtenidas de _contextData
                    Area = areas.FirstOrDefault(a => a.Id == x.pnl.AreaId)?.AreaDescription,
                    Location = locations.FirstOrDefault(l => l.Id == x.pnl.LocationId)?.LocationDescription,
                    PartNumber = parts.FirstOrDefault(p => p.Id == x.pnl.PartNumberId)?.PartNumberName
                }
            }).ToList();
        }

        public async Task<ComponentAlertResponseDto?> GetById(Guid id)
        {
            // Paso 1: Obtener datos del primer contexto
            var baseRecord = await (from ca in _context.ComponentAlerts
                                    where ca.Id == id
                                    join pnl in _context.PartNumberLogistics on ca.PartNumberLogisticsId equals pnl.Id
                                    select new { ca, pnl }).FirstOrDefaultAsync();

            if (baseRecord == null) return null;

            // Paso 2: Consultar detalles específicos en el segundo contexto
            var area = await _contextData.ProductionAreas.FindAsync(baseRecord.pnl.AreaId);
            var location = await _contextData.ProductionLocations.FindAsync(baseRecord.pnl.LocationId);
            var part = await _contextData.ProductionPartNumbers.FindAsync(baseRecord.pnl.PartNumberId);

            // Paso 3: Mapear resultado
            return new ComponentAlertResponseDto
            {
                Id = baseRecord.ca.Id,
                Active = baseRecord.ca.Active,
                CreateDate = baseRecord.ca.CreateDate,
                CreateBy = baseRecord.ca.CreateBy,
                UpdateDate = baseRecord.ca.UpdateDate,
                UpdateBy = baseRecord.ca.UpdateBy,
                CompleteBy = baseRecord.ca.CompleteBy,
                CompleteDate = baseRecord.ca.CompleteDate,
                ReceivedBy = baseRecord.ca.ReceivedBy,
                ReceivedDate = baseRecord.ca.ReceivedDate,
                CancelBy = baseRecord.ca.CancelBy,
                CancelDate = baseRecord.ca.CancelDate,
                CriticalBy = baseRecord.ca.CriticalBy,
                CriticalDate = baseRecord.ca.CriticalDate,
                PartNumberLogisticsId = baseRecord.ca.PartNumberLogisticsId,
                StatusDescrition = GetStatusDescription(baseRecord.ca),
                PartNumberLogisticsResponseDto = new PartNumberLogisticsResponseDto
                {
                    Id = baseRecord.pnl.Id,
                    Active = baseRecord.pnl.Active,
                    CreateBy = baseRecord.pnl.CreateBy,
                    CreateDate = baseRecord.pnl.CreateDate,
                    UpdateBy = baseRecord.pnl.UpdateBy,
                    UpdateDate = baseRecord.pnl.UpdateDate,
                    Area = area?.AreaDescription,
                    Location = location?.LocationDescription,
                    PartNumber = part?.PartNumberName
                }
            };
        }

        public async Task<ComponentAlertResponseDto> Update(Guid id, ComponentAlertRequestDto updateDto)
        {
            var alert = await _context.ComponentAlerts.FindAsync(id);
            if (alert == null)
            {
                throw new KeyNotFoundException("Alert not found");
            }

            alert.Active = updateDto.Active;
            alert.UpdateBy = updateDto.UpdateBy;
            alert.UpdateDate = DateTime.Now;
            alert.CompleteBy = updateDto.CompleteBy;
            alert.CompleteDate = updateDto.CompleteDate;
            alert.ReceivedBy = updateDto.ReceivedBy;
            alert.ReceivedDate = updateDto.ReceivedDate;
            alert.CancelBy = updateDto.CancelBy;
            alert.CancelDate = updateDto.CancelDate;
            alert.CriticalBy = updateDto.CriticalBy;
            alert.CriticalDate = updateDto.CriticalDate;
            alert.PartNumberLogisticsId = updateDto.PartNumberLogisticsId;

            _context.ComponentAlerts.Update(alert);
            await _context.SaveChangesAsync();

            return (await GetById(id))!;
        }

        private string GetStatusDescription(ComponentAlert alert)
        {
            if (alert.CancelDate.HasValue) return "Cancelada";
            if (alert.CompleteDate.HasValue) return "Completada";
            if (alert.CriticalDate.HasValue) return "Critica";
            if (alert.ReceivedDate.HasValue) return "Recibida";
            return "Abierta";
        }
    }
}