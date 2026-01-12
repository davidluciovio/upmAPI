using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.AplicationDtos.DowntimeCapture
{
    public class DowntimeCaptureRequestDto
    {
        public DateTime StartDatetime { get; set; }
        public DateTime EndDatetime { get; set; }

        public List<ProductionStation> ProductionStations { get; set; } = new List<ProductionStation>();

        public class ProductionStation
        {
            public Guid LineId { get; set; }
            public Guid ModelId { get; set; }
        }
    }
}
