using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upmData.Context;
using upmData.Models;
using upmDomain.Interfaces;
using upmDomain.WorkShifts;

namespace upmDomain.Shift
{
    public class WorkShiftService : IService<WorkShiftDto>
    {
        private readonly UpmwebContext _context;

        public WorkShiftService(UpmwebContext context)
        {
            _context = context;
        }

        public WorkShiftDto CalculateShift(DateTime datetime)
        {
            var now = datetime;
            var current = TimeOnly.FromDateTime(datetime); // <- TimeOnly

            var shift = _context.WorkShifts
                .Where(s => s.Active)
                .FirstOrDefault(x =>
                x.StartTime < x.EndTime
                    ? x.StartTime <= current && current < x.EndTime      // turno normal
                    : current >= x.StartTime || current < x.EndTime       // cruza medianoche
            );

            if (shift is null)
                throw new InvalidOperationException("No se encontró un turno válido para la hora actual.");

            var shiftDate = (shift.EndTime > shift.StartTime)
                ? DateOnly.FromDateTime(now)
                : DateOnly.FromDateTime(now.AddDays(-1));

            var startDateTime = shiftDate.ToDateTime(shift.StartTime); // DateTime exacto de inicio

            return new WorkShiftDto
            {
                WorkShiftId = shift.Id,
                StartTime = startDateTime,
                EndTime = now,
                ReferenceDate = now,
                Description = shift.Description,
            };
        }

        public async Task<List<WorkShiftDto>> GetAllAsync()
        {
            var now = DateTime.Now.Date;
            return await _context.WorkShifts
                .Where(ws => ws.Active && ws.Id != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                .Select(ws => new WorkShiftDto
                {
                    WorkShiftId = ws.Id,
                    Description = ws.Description,
                    EndTime = new DateTime(now.Year, now.Month, now.Day, ws.StartTime.Hour, ws.StartTime.Minute, ws.StartTime.Second).AddSeconds(ws.SecondsQuantity),
                    ReferenceDate = now,
                    SecondsQuantity = ws.SecondsQuantity,
                    StartTime = new DateTime(now.Year, now.Month, now.Day, ws.StartTime.Hour, ws.StartTime.Minute, ws.StartTime.Second)
                }).ToListAsync();
        }
    }
}
