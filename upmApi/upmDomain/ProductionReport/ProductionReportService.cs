using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upmData.Context;
using upmDomain.Interfaces;
using upmDomain.Lider;
using upmDomain.LineDomain;
using upmDomain.Shift;

namespace upmDomain.ProductionReport
{
    public class ProductionReportService : IService<ProductionReportDto>
    {
        private readonly UpmwebContext _context;
        private ShiftService _shiftService;
        private LiderService _liderService;
        public ProductionReportService(UpmwebContext context, ShiftService shiftService, LiderService liderService) 
        {
            _context = context;
            _liderService = liderService;
            _shiftService = shiftService;
        }

        public async Task<List<ProductionReportDto>> GetAllAsync()
        {
            // Cargar data de soporte en memoria (solo lo que necesites)
            var lines = await _context.Lines
                .Where(l => l.Active)
                .ToDictionaryAsync(l => l.Id);

            var shifts = await _context.WorkShifts
                .Where(ws => ws.Active)
                .ToDictionaryAsync(ws => ws.Id);

            var liderConfigurations = await _context.LiderConfigurations
                .Where(lc => lc.Active)
                .GroupBy(lc => lc.PartNumberConfigurationId)
                .ToDictionaryAsync(g => g.Key, g => g.First().UserId);

            var liders = (await _liderService.GetAllAsync())
                .ToDictionary(ld => ld.LiderId);

            // Obtener los ProductionRegisters (mínima data necesaria)
            var productionRegisters = await _context.ProductionRegisters
                .Where(pr => pr.Active)
                .Select(pr => new
                {
                    pr.CreateDate,
                    LineId = pr.PartNumberConfiguration.LineId,
                    pr.PartNumberConfigurationId
                })
                .ToListAsync();

            // Agrupar en memoria (ya con la info necesaria)
            var grouped = productionRegisters
                .GroupBy(pr => new
                {
                    Date = new DateTime(pr.CreateDate.Year, pr.CreateDate.Month, pr.CreateDate.Day),
                    ShiftId = _shiftService.GetShift(pr.CreateDate).WorkShiftId,
                    LineId = pr.LineId,
                    LiderId = liderConfigurations.TryGetValue(pr.PartNumberConfigurationId, out var lid) ? lid : Guid.Empty
                });

            // Proyectar a DTO
            var reports = grouped.Select(group =>
            {
                var shift = shifts[group.Key.ShiftId];
                var line = lines[group.Key.LineId];
                var lider = liders.TryGetValue(group.Key.LiderId, out var ld) ? ld : null;

                return new ProductionReportDto
                {
                    StartDatetime = group.Key.Date,
                    EndDatetime = group.Key.Date.AddSeconds(shift.SecondsQuantity),
                    Shift = shift.Description,
                    Line = LineMapper.Map(line),
                    Lider = lider
                };
            }).ToList();

            return reports;
        }

    }
}
