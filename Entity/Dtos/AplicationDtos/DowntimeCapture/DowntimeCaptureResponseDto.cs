using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.AplicationDtos.DowntimeCapture
{
    public class DowntimeCaptureResponseDto
    {
        public Guid LineId { get; set; }
        public string LineDescription { get; set; } = string.Empty;
        public List<PartNumberDataProduction> partNumberDataProductions { get; set; } = new List<PartNumberDataProduction>();

        public class PartNumberDataProduction
        {
            public Guid PartNumberId { get; set; }
            public string PartNumberName { get; set; } = string.Empty;
            public string PartNumberDescription { get; set; } = string.Empty;
            public Guid ModelId { get; set; }

            public string ModelName { get; set; } = string.Empty;

            public float ObjetiveTime { get; set; }
            public float HPTime { get; set; }

            public List<HourlyProductionData> hourlyProductionDatas { get; set; } = new List<HourlyProductionData>();

            public class HourlyProductionData
            {
                public DateTime StartProductionDate { get; set; }
                public DateTime EndProductionDate { get; set; }
                public float DowntimeP { get; set; }
                public float DowntimeNP { get; set; }
                public float TotalDowntime { get; set; }
                public float TotalWorkingTime { get; set; }
                public float MinutesPzas { get; set; }
                public float ProducedQuantity { get; set; }
                public float ObjetiveQuantity { get; set; }
                public float Efectivity { get; set; }

            }

            public class OperatorsDto
            {
                public Guid LineOperatorId { get; set; }
                public string OperatorCode { get; set; } = string.Empty;
                public string OperatorName { get; set; } = string.Empty;
                public DateTime StartDatetime { get; set; }
                public DateTime EndDatetime { get; set; }
            }
        }
    }
}
