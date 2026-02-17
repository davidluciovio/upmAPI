using System;

namespace Entity.Dtos.ModelDtos.ProductionControl.PartNumberStructure
{
    public class PartNumberStructureRequestDto
    {
        public string CreateBy { get; set; } = string.Empty;
        public string UpdateBy { get; set; } = string.Empty;
        public bool Active { get; set; }
        public Guid PartNumberLogisticId { get; set; }
        public Guid ProductionStationId { get; set; }
        public Guid MaterialSuplierId { get; set; }
        public string PartNumberName { get; set; } = string.Empty;
        public string PartNumberDescription { get; set; } = string.Empty;
    }
}
