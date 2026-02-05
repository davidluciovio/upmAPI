using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.AplicationDtos.OperationalAnalysis
{
    public class OperationalAnalysisRequestDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> Leaders { get; set; } = new List<string>();
        public List<string> Managments { get; set; } = new List<string>();
        public List<string> Areas { get; set; } = new List<string>();
        public List<string> Supervisors { get; set; } = new List<string>();
        public List<string> Jefes { get; set; } = new List<string>();
        public List<string> PartNumbers { get; set; } = new List<string>();
        public List<string> Shifts { get; set; } = new List<string>();
    }
}
