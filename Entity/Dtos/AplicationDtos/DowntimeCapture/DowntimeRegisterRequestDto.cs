using System;

namespace Entity.Dtos.AplicationDtos.DowntimeCapture
{
    public class DowntimeRegisterRequestDto
    {
        public bool Active { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public string UpdateBy { get; set; } = string.Empty;

        public DateTime StartDowntimeDatetime { get; set; }
        public DateTime EndDowntimeDatetime { get; set; }

        public Guid DataProductionDowntimeId { get; set; }

        public Guid ProductionStationId { get; set; }

    }
}
