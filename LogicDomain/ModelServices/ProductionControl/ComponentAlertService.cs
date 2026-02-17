using Entity.Dtos.ModelDtos.ProductionControl.ComponentAlert;
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
    public class ComponentAlertService : IComponentAlertService
    {
        private readonly ProductionControlContext _context;
        private readonly string _user = "System"; // This should be replaced with a real user from context

        public ComponentAlertService(ProductionControlContext context)
        {
            _context = context;
        }

        public async Task<ComponentAlertDto> Create(ComponentAlertCreateDto dtocreate)
        {
            var alert = new ComponentAlert
            {
                Id = Guid.NewGuid(),
                PartNumberLogisticsId = dtocreate.PartNumberLogisticsId,
                CreateDate = DateTime.UtcNow,
                CreateBy = _user,
                Active = true,
                StatusId = Guid.NewGuid() // Placeholder for 'New' status
            };

            _context.ComponentAlerts.Add(alert);
            await _context.SaveChangesAsync();

            return MapToDto(alert);
        }

        public async Task<List<ComponentAlertDto>> GetAlls()
        {
            var alerts = await _context.ComponentAlerts.Where(a => a.Active).ToListAsync();
            return alerts.Select(MapToDto).ToList();
        }

        public async Task<ComponentAlertDto?> GetById(Guid id)
        {
            var alert = await _context.ComponentAlerts.FirstOrDefaultAsync(a => a.Id == id && a.Active);
            return alert != null ? MapToDto(alert) : null;
        }

        public async Task<ComponentAlertDto> Update(Guid id, ComponentAlertUpdateDto updateDto)
        {
            var alert = await _context.ComponentAlerts.FindAsync(id);
            if (alert == null)
            {
                throw new Exception("Alert not found");
            }

            if (updateDto.Active.HasValue)
            {
                alert.Active = updateDto.Active.Value;
            }
            if (updateDto.StatusId.HasValue)
            {
                alert.StatusId = updateDto.StatusId.Value;
            }

            _context.Entry(alert).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return MapToDto(alert);
        }

        public async Task<bool> Delete(Guid id)
        {
            var alert = await _context.ComponentAlerts.FindAsync(id);
            if (alert == null)
            {
                return false;
            }

            alert.Active = false;
            _context.Entry(alert).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        private ComponentAlertDto MapToDto(ComponentAlert alert)
        {
            return new ComponentAlertDto
            {
                Id = alert.Id,
                Active = alert.Active,
                CreateDate = alert.CreateDate,
                CreateBy = alert.CreateBy,
                CompleteBy = alert.CompleteBy,
                CompleteDate = alert.CompleteDate,
                ReceivedBy = alert.ReceivedBy,
                ReceivedDate = alert.ReceivedDate,
                CancelBy = alert.CancelBy,
                CancelDate = alert.CancelDate,
                CriticalBy = alert.CriticalBy,
                CriticalDate = alert.CriticalDate,
                StatusId = alert.StatusId,
                PartNumberLogisticsId = alert.PartNumberLogisticsId
            };
        }
    }
}