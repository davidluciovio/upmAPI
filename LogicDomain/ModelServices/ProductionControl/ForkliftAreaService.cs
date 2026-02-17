using Entity.Dtos.ModelDtos.ProductionControl.ForkliftArea;
using Entity.Models.Auth;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicDomain.ModelServices.ProductionControl
{
    public class ForkliftAreaService
    {
        private readonly ProductionControlContext _contextProductionControl;
        private readonly AuthContext _contextAuth;
        private readonly DataContext _contextData;

        public ForkliftAreaService(ProductionControlContext contextProductionControl, AuthContext authContext, DataContext dataContext)
        {
            _contextProductionControl = contextProductionControl;
            _contextAuth = authContext;
            _contextData = dataContext;
        }

        public async Task AddForkliftAreaAsync(Guid userId, List<Guid> dataProductionAreaIds)
        {
            // 1. Traemos TODO lo que el usuario ya tiene (activos e inactivos)
            // Esto es para decidir si "insertamos" o "reactivamos"
            var allExistingRecords = await _contextProductionControl.ForkliftAreas
                .Where(f => f.UserId == userId)
                .ToListAsync();

            // 2. ELIMINAR/DESACTIVAR: Registros que están en DB pero NO vienen en el nuevo arreglo
            var toRemove = allExistingRecords
                .Where(f => f.Active && !dataProductionAreaIds.Contains(f.DataProductionAreaId))
                .ToList();

            foreach (var item in toRemove)
            {
                item.Active = false; // Usando borrado lógico por seguridad
                item.UpdateDate = DateTime.UtcNow;
                item.UpdateBy = "System";
            }

            // 3. AGREGAR O REACTIVAR:
            foreach (var areaId in dataProductionAreaIds)
            {
                var existing = allExistingRecords.FirstOrDefault(f => f.DataProductionAreaId == areaId);

                if (existing == null)
                {
                    // SI NO EXISTE: Se crea el nuevo registro
                    await _contextProductionControl.ForkliftAreas.AddAsync(new LogicData.Models.ProductionControl.ForkliftArea
                    {
                        Id = Guid.NewGuid(),
                        Active = true,
                        CreateDate = DateTime.UtcNow,
                        CreateBy = "System",
                        UpdateDate = DateTime.UtcNow,
                        UpdateBy = "System",
                        UserId = userId,
                        DataProductionAreaId = areaId
                    });
                }
                else if (!existing.Active)
                {
                    // SI EXISTE PERO ESTABA INACTIVO: Se reactiva en lugar de duplicar
                    existing.Active = true;
                    existing.UpdateDate = DateTime.UtcNow;
                    existing.UpdateBy = "System";
                }
                // SI EXISTE Y YA ESTÁ ACTIVO: No hacemos nada (aquí se cumple tu condición)
            }

            await _contextProductionControl.SaveChangesAsync();
        }

        public async Task<List<ForkliftAreaResponseDto>> GetForkliftAreasByUserIdAsync(Guid userId)
        {

            // 1. Obtenemos los datos de ForkliftAreas (ProductionControlContext)
            var forkliftAreas = await _contextProductionControl.ForkliftAreas
                .Where(f => f.UserId == userId && f.Active)
                .ToListAsync();

            if (!forkliftAreas.Any()) return new List<ForkliftAreaResponseDto>();

            // 2. Obtenemos el usuario (AuthContext)
            var user = await _contextAuth.Users
                .Where(u => u.Id == userId.ToString())
                .Select(u => u.UserName)
                .FirstOrDefaultAsync();

            // 3. Obtenemos las áreas de producción relacionadas (DataContext)
            var areaIds = forkliftAreas.Select(f => f.DataProductionAreaId).Distinct().ToList();
            var productionAreas = await _contextData.ProductionAreas
                .Where(p => areaIds.Contains(p.Id))
                .Select(p => new ForkliftAreaResponseDto.AreasData
                {
                    Id = p.Id,
                    AreaName = p.AreaDescription
                })
                .ToListAsync();

            // 4. Unimos todo en memoria
            var response = new ForkliftAreaResponseDto
            {
                UserId = userId,
                UserName = user ?? "Unknown",
                DataProductionAreaId = productionAreas // Asignamos la lista de áreas encontradas
            };

            return new List<ForkliftAreaResponseDto> { response };
        }
    }
}
