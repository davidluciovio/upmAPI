using System;

namespace Entity.Dtos.AssyProduction
{
    public class ProductionStationUpdateDto
    {
        public Guid PartNumberId { get; set; }
        public Guid LineId { get; set; }
        public bool Active { get; set; }
        public string UpdateBy { get; set; } = string.Empty;
    }
}
