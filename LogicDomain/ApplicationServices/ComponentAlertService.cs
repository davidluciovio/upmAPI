using Entity.Dtos.AplicationDtos.ComponentAlert;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicDomain.ApplicationServices
{
    public class ComponentAlertService
    {
        private readonly ProductionControlContext _productionControlContext;
        private readonly DataContext _dataContext;
        private readonly AuthContext _authContext;

        public ComponentAlertService(ProductionControlContext productionControlContext, DataContext dataContext, AuthContext authContext)
        {
            _productionControlContext = productionControlContext;
            _dataContext = dataContext;
            _authContext = authContext;
        }

        public async Task<ComponentAlertResponseDto> UpdateComponentAlert(Guid id, ComponentAlertRequestDto updateDto)
        {
            var existingAlert = await _productionControlContext.ComponentAlerts.FindAsync(id);
            if (existingAlert == null || !existingAlert.Active)
            {
                throw new Exception("Component Alert not found or inactive.");
            }
            existingAlert.UpdateBy = updateDto.UpdateBy;
            existingAlert.UpdateDate = DateTime.Now;
            existingAlert.CancelBy = updateDto.CancelBy;
            existingAlert.CancelDate = updateDto.CancelDate;
            existingAlert.CompleteBy = updateDto.CompleteBy;
            existingAlert.CompleteDate = updateDto.CompleteDate;
            existingAlert.CriticalBy = updateDto.CriticalBy;
            existingAlert.CriticalDate = updateDto.CriticalDate;
            existingAlert.ReceivedBy = updateDto.ReceivedBy;
            existingAlert.ReceivedDate = updateDto.ReceivedDate;
            existingAlert.PartNumberLogisticsId = updateDto.PartNumberLogisticsId;
            existingAlert.UserId = updateDto.UserId;

            await _productionControlContext.SaveChangesAsync();
            return await GetComponentAlertById(existingAlert.Id);
        }

        public async Task<ComponentAlertResponseDto> CreateComponentAlert(ComponentAlertRequestDto createDto)
        {

            var createdStatus = await _dataContext.Statuses.FirstOrDefaultAsync(s => s.StatusDescription == "CREATED");

            if (createdStatus == null)
            {
                createdStatus = new Entity.Models.DataUPM.DataStatus
                {
                    CreateDate = DateTime.Now,
                    Active = true,
                    CreateBy = "System",
                    Id = Guid.NewGuid(),
                    StatusDescription = "CREATED"
                };

                _dataContext.Statuses.Add(createdStatus);
                await _dataContext.SaveChangesAsync();
            }
            
            var newAlert = new Entity.Models.ProductionControl.ComponentAlert
            {
                Active = true,
                CreateBy = createDto.CreateBy,
                CreateDate = DateTime.Now,
                UpdateBy = createDto.CreateBy,
                UpdateDate = DateTime.Now,
                PartNumberLogisticsId = createDto.PartNumberLogisticsId,
                StatusId = createdStatus.Id,
                UserId = createDto.UserId
            };
            _productionControlContext.ComponentAlerts.Add(newAlert);
            await _productionControlContext.SaveChangesAsync();
            return await GetComponentAlertById(newAlert.Id);
        }

        public async Task<ComponentAlertResponseDto> GetComponentAlertById(Guid id)
        {
            // 1. Traemos la base de la alerta desde ProductionControl
            var ca = await _productionControlContext.ComponentAlerts
                .Include(ca => ca.PartNumberLogistics)
                .FirstOrDefaultAsync(ca => ca.Id == id && ca.Active);

            if (ca == null) return null;

            // 2. Consultamos los datos externos de forma INDEPENDIENTE
            var area = await _dataContext.ProductionAreas
                .Where(a => a.Id == ca.PartNumberLogistics.AreaId)
                .Select(a => a.AreaDescription).FirstOrDefaultAsync();

            var location = await _dataContext.ProductionLocations
                .Where(l => l.Id == ca.PartNumberLogistics.LocationId)
                .Select(l => l.LocationDescription).FirstOrDefaultAsync();

            var partNumber = await _dataContext.ProductionPartNumbers
                .Where(p => p.Id == ca.PartNumberLogistics.PartNumberId)
                .Select(p => p.PartNumberName).FirstOrDefaultAsync();

            var statusDesc = await _dataContext.Statuses
                .Where(s => s.Id == ca.StatusId)
                .Select(s => s.StatusDescription).FirstOrDefaultAsync();

            var userName = await _authContext.Users
                .Where(u => u.Id == ca.UserId)
                .Select(u => u.PrettyName).FirstOrDefaultAsync();

            // 3. Mapeamos manualmente al DTO
            return new ComponentAlertResponseDto
            {
                Id = ca.Id,
                PartNumberLogistics = new Entity.Dtos.ProductionControl.PartNumberLogisticsResponseDto
                {
                    Id = ca.PartNumberLogistics.Id,
                    Active = ca.PartNumberLogistics.Active,
                    Area = area,
                    Location = location,
                    PartNumber = partNumber,
                    SNP = ca.PartNumberLogistics.SNP,
                    CreateBy = ca.PartNumberLogistics.CreateBy,
                    CreateDate = ca.PartNumberLogistics.CreateDate,
                    UpdateBy = ca.PartNumberLogistics.UpdateBy,
                    UpdateDate = ca.PartNumberLogistics.UpdateDate
                },
                CreateBy = ca.CreateBy,
                CreateDate = ca.CreateDate,
                UpdateBy = ca.UpdateBy,
                UpdateDate = ca.UpdateDate,
                CancelBy = ca.CancelBy,
                CancelDate = ca.CancelDate,
                CompleteBy = ca.CompleteBy,
                CompleteDate = ca.CompleteDate,
                CriticalBy = ca.CriticalBy,
                CriticalDate = ca.CriticalDate,
                ReceivedBy = ca.ReceivedBy,
                ReceivedDate = ca.ReceivedDate,
                Status = statusDesc,
                User = userName
            };
        }

        public async Task<List<ComponentAlertResponseDto>> GetAllComponentAlerts()
        {

            var partNumbers = await _dataContext.ProductionPartNumbers.ToDictionaryAsync(p => p.Id, p => p);
            var areas = await _dataContext.ProductionAreas.ToDictionaryAsync(a => a.Id, a => a); 
            var locations = await _dataContext.ProductionLocations.ToDictionaryAsync(l => l.Id, l => l);
            var statuses = await _dataContext.Statuses.ToDictionaryAsync(s => s.Id, s => s);
            var users = await _authContext.Users.ToDictionaryAsync(u => u.Id, u => u);


            var componentAlerts = await _productionControlContext.ComponentAlerts
                .Include(ca => ca.PartNumberLogistics)
                .Where(ca => ca.Active)
                .Select(ca => new ComponentAlertResponseDto
                {
                    Id = ca.Id,
                    PartNumberLogistics = new Entity.Dtos.ProductionControl.PartNumberLogisticsResponseDto
                    {
                        Active = ca.PartNumberLogistics.Active,
                        Area = areas[ca.PartNumberLogistics.AreaId].AreaDescription,
                        Location = locations[ca.PartNumberLogistics.LocationId].LocationDescription,
                        PartNumber = partNumbers[ca.PartNumberLogistics.PartNumberId].PartNumberName,
                        CreateBy = ca.PartNumberLogistics.CreateBy,
                        CreateDate = ca.PartNumberLogistics.CreateDate,
                        UpdateBy = ca.PartNumberLogistics.UpdateBy,
                        UpdateDate = ca.PartNumberLogistics.UpdateDate,
                        SNP = ca.PartNumberLogistics.SNP,
                        Id = ca.PartNumberLogistics.Id,
                    },
                    CreateBy = ca.CreateBy,
                    CreateDate = ca.CreateDate,
                    UpdateBy = ca.UpdateBy,
                    UpdateDate = ca.UpdateDate,
                    CancelBy = ca.CancelBy,
                    CancelDate = ca.CancelDate,
                    CompleteBy = ca.CompleteBy,
                    CompleteDate = ca.CompleteDate,
                    CriticalBy = ca.CriticalBy,
                    CriticalDate = ca.CriticalDate,
                    ReceivedBy = ca.ReceivedBy,
                    ReceivedDate = ca.ReceivedDate,
                    Status = statuses[ca.StatusId].StatusDescription,
                    User = users[ca.UserId].PrettyName
                })
                .ToListAsync();
            return componentAlerts;
        }
    }
}
