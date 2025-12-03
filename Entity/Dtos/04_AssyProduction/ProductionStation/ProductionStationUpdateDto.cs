using System;

namespace Entity.Dtos.AssyProduction
{
    public class ProductionStationUpdateDto
    {
        public Guid PartNumberId { get; set; }
        public Guid LineId { get; set; }
        public Guid ModelId { get; set; }
        public bool Active { get; set; }
        public string UpdateBy { get; set; } = string.Empty;

        public float ObjetiveTime { get; set; }
        public float NetoTime { get; set; }
        public int OperatorQuantity { get; set; }
        public int PartNumberQuantity { get; set; }
    }
}
