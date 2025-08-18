using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upmData.Models;

namespace upmDomain.WorkShifts
{
    public class WorkShiftDto
    {
        public Guid WorkShiftId { get; set; }
        public DateTime ReferenceDate { get; set; }
        public string Description { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int SecondsQuantity { get; set; }
    }
}
