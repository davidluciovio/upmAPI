using System;

namespace Entity.Dtos.AssyProduction
{
    public class ProductionStationCreateDto
    {
        public Guid PartNumberId { get; set; }
        public Guid LineId { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public string UpdateBy { get; set; } = string.Empty;
    }
}
