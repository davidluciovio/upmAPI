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

    }
}
