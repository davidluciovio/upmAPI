using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upmData.Context;
using upmData.Models;

namespace upmDomain.Shift
{
    public class ShiftService
    {
        private readonly UpmwebContext _context;

        public ShiftService()
        {
        }

        public ShiftService(UpmwebContext context)
        {
            _context = context;
        }

        //public ShiftDto GetShift(DateTime datetime)
        //{
        //    var currentTime = datetime.TimeOfDay;

        //    var shift = _context.WorkShifts
        //        .Where(s => s.Active)
        //        .FirstOrDefault(x =>
        //            x.StartTime < x.EndTime
        //                ? (x.StartTime.Hour <= currentTime.Hours && currentTime.Hours < x.EndTime.Hour) // turno normal
        //                : (currentTime.Hours >= x.StartTime.Hour || currentTime.Hours < x.EndTime.Hour) // turno que cruza medianoche
        //        );

        //    if (shift == null)
        //        throw new InvalidOperationException("No se encontró un turno válido para la hora actual.");

        //    // Determinar la fecha de inicio del turno
        //    var shiftDate = (shift.EndTime > shift.StartTime) ? datetime.Date : datetime.AddDays(-1).Date;

        //    var startTime = shiftDate
        //        .AddHours(shift.StartTime.Hour)
        //        .AddMinutes(shift.StartTime.Minute)
        //        .AddSeconds(shift.StartTime.Second);

        //    return new ShiftDto
        //    {
        //        WorkShiftId = shift.Id,
        //        StartTime = startTime,
        //        EndTime = datetime,
        //        ReferenceDate = datetime.Date,
        //        ShiftDetails = shift
        //    };
        //}

        public ShiftDto GetShift(DateTime datetime)
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

            return new ShiftDto
            {
                WorkShiftId = shift.Id,
                StartTime = startDateTime,
                EndTime = now,
                ReferenceDate = now,
                ShiftDetails = shift
            };
        }


    }
}
