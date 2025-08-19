using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upmDomain.DomainModel;
using upmDomain.Lider;
using upmDomain.LineDomain;
using upmDomain.WorkShifts;

namespace upmDomain.DomainProductionReport
{
    public class ProductionReportListDto
    {
        public LiderDto Lider { get; set; } = new LiderDto();
        public LineDto  Line { get; set; } = new LineDto();
        public ModelDto Model { get; set; } = new ModelDto();
        public WorkShiftDto Shift { get; set; } = new WorkShiftDto();
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
