using System;

namespace Entity.Dtos.AssyProduction
{
    public class ProductionStationDto
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; } = string.Empty;

        public string PartNumber { get; set; } = string.Empty;
        public string Line { get; set; } = string.Empty;
    }
}
