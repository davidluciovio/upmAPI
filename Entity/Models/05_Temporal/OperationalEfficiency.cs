using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models._05_Temporal
{
    public class OperationalEfficiency
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;

        public DateTime ProductionDate { get; set; }
        public string Area { get; set; } = string.Empty;
        public string Supervisor { get; set; } = string.Empty;
        public string Leader { get; set; } = string.Empty;
        public string Shift { get; set; } = string.Empty;
        public string PartNumberName { get; set; } = string.Empty;

        public float HP { get; set; }
        public float Neck { get; set; }

        public Guid? PartNumberId { get; set; }


        public float RealTime { get; set; }
        public float OperativityPercent { get; set; }
        public float PriductionReal { get; set; }
        public float TotalTime { get; set; }
        public float ProgramabeDowntimeTime { get; set; }
        public float RealWorkingTime { get; set; }
        public float NetoWorkingTime { get; set; }
        public float NetoProduictiveTime { get; set; }
        public float TotalDowntime { get; set; }
        public float NoProgramabeDowntimeTime { get; set; }
        public float NoReportedTime { get; set; }
        public float DowntimePercent { get; set; }
        public float NoProgramableDowntimePercent { get; set; }
        public float ProgramableDowntimePercent { get; set; }

    }
}
