using System;

namespace Entity.Dtos.AssyProduction
{
    public class ProductionStationCreateDto
    {
        public Guid PartNumberId { get; set; }
        public Guid LineId { get; set; }
        public Guid ModelId { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public string UpdateBy { get; set; } = string.Empty;

        public float ObjetiveTime { get; set; }
        public float NetoTime { get; set; }
        public int OperatorQuantity { get; set; }
        public int PartNumberQuantity { get; set; }
    }
}
