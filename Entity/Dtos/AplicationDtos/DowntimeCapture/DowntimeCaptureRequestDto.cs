using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.AplicationDtos.DowntimeCapture
{
    public class DowntimeCaptureRequestDto
    {
        public DateTime StartDatetime { get; set; }
        public DateTime EndDatetime { get; set; }
        public string LineDescription { get; set; } = string.Empty;

        public class OperatorsDto
        {
            public string OperatorCode { get; set; } = string.Empty;
            public string OperatorName { get; set; } = string.Empty;

            public Guid LineId { get; set; }

            public DateTime StartDatetime { get; set; }
            public DateTime EndDatetime { get; set; }
        }

    }
}
