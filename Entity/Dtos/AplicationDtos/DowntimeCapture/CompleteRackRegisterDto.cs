using System;

namespace Entity.Dtos.AplicationDtos.DowntimeCapture
{
    public class CompleteRackRegisterDto
    {
        public string NoRack { get; set; } = string.Empty;
        public string Serie { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public Guid ProductionStationId { get; set; }
    }
}
