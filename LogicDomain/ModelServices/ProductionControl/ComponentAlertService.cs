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
    public class ComponentAlertService : IServiceCrud<ComponentAlertResponseDto, ComponentAlertRequestDto, ComponentAlertRequestDto>
    {
        private readonly ProductionControlContext _context;
        private readonly DataContext _contextData;

        public ComponentAlertService(ProductionControlContext context, DataContext contextData)
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
            var alerts = await (from ca in _context.ComponentAlerts
                                join pnl in _context.PartNumberLogistics on ca.PartNumberLogisticsId equals pnl.Id
                                join pa in _contextData.ProductionAreas on pnl.AreaId equals pa.Id
                                join pl in _contextData.ProductionLocations on pnl.LocationId equals pl.Id
                                join pn in _contextData.ProductionPartNumbers on pnl.PartNumberId equals pn.Id
                                select new ComponentAlertResponseDto
                                {
                                    Id = ca.Id,
                                    Active = ca.Active,
                                    CreateDate = ca.CreateDate,
                                    CreateBy = ca.CreateBy,
                                    UpdateDate = ca.UpdateDate,
                                    UpdateBy = ca.UpdateBy,
                                    CompleteBy = ca.CompleteBy,
                                    CompleteDate = ca.CompleteDate,
                                    ReceivedBy = ca.ReceivedBy,
                                    ReceivedDate = ca.ReceivedDate,
                                    CancelBy = ca.CancelBy,
                                    CancelDate = ca.CancelDate,
                                    CriticalBy = ca.CriticalBy,
                                    CriticalDate = ca.CriticalDate,
                                    PartNumberLogisticsId = ca.PartNumberLogisticsId,
                                    StatusDescrition = GetStatusDescription(ca),
                                    PartNumberLogisticsResponseDto = new PartNumberLogisticsResponseDto
                                    {
                                        Active = pnl.Active,
                                        CreateBy = pnl.CreateBy,
                                        CreateDate = pnl.CreateDate,
                                        Id = pnl.Id,
                                        Location = pl.LocationDescription,
                                        Area = pa.AreaDescription,
                                        PartNumber = pn.PartNumberName,
                                        UpdateBy = pnl.UpdateBy,
                                        UpdateDate = pnl.UpdateDate,
                                    }
                                }).ToListAsync();
            return alerts;
        }

        public async Task<ComponentAlertResponseDto?> GetById(Guid id)
        {
            var alert = await (from ca in _context.ComponentAlerts
                               where ca.Id == id
                               join pnl in _context.PartNumberLogistics on ca.PartNumberLogisticsId equals pnl.Id
                               join pa in _contextData.ProductionAreas on pnl.AreaId equals pa.Id
                               join pl in _contextData.ProductionLocations on pnl.LocationId equals pl.Id
                               join pn in _contextData.ProductionPartNumbers on pnl.PartNumberId equals pn.Id
                               select new ComponentAlertResponseDto
                               {
                                   Id = ca.Id,
                                   Active = ca.Active,
                                   CreateDate = ca.CreateDate,
                                   CreateBy = ca.CreateBy,
                                   UpdateDate = ca.UpdateDate,
                                   UpdateBy = ca.UpdateBy,
                                   CompleteBy = ca.CompleteBy,
                                   CompleteDate = ca.CompleteDate,
                                   ReceivedBy = ca.ReceivedBy,
                                   ReceivedDate = ca.ReceivedDate,
                                   CancelBy = ca.CancelBy,
                                   CancelDate = ca.CancelDate,
                                   CriticalBy = ca.CriticalBy,
                                   CriticalDate = ca.CriticalDate,
                                   PartNumberLogisticsId = ca.PartNumberLogisticsId,
                                   StatusDescrition = GetStatusDescription(ca),
                                   PartNumberLogisticsResponseDto = new PartNumberLogisticsResponseDto
                                   {
                                       Active = pnl.Active,
                                       CreateBy = pnl.CreateBy,
                                       CreateDate = pnl.CreateDate,
                                       Id = pnl.Id,
                                       Location = pl.LocationDescription,
                                       Area = pa.AreaDescription,
                                       PartNumber = pn.PartNumberName,
                                       UpdateBy = pnl.UpdateBy,
                                       UpdateDate = pnl.UpdateDate,
                                   }
                               }).FirstOrDefaultAsync();
            return alert;
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