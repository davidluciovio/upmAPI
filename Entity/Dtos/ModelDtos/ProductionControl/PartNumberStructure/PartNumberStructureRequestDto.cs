using System;

namespace Entity.Dtos.ModelDtos.ProductionControl.PartNumberStructure
{
    public class PartNumberStructureRequestDto
    {
        public Guid PartNumberLogisticId { get; set; }
        public Guid CompletePartId { get; set; }
        public string CompletePartName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public Guid MaterialSuplierId { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public string UpdateBy { get; set; } = string.Empty;
        public bool Active { get; set; }
    }
}
