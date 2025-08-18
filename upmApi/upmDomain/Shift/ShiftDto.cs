using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upmData.Models;

namespace upmDomain.Shift
{
    public class ShiftDto
    {
        public Guid WorkShiftId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime ReferenceDate { get; set; }
        public WorkShift? ShiftDetails { get; set; }
    }
}
