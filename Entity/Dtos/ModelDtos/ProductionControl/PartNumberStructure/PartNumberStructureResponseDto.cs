using System;

namespace Entity.Dtos.ModelDtos.ProductionControl.PartNumberStructure
{
    public class PartNumberStructureResponseDto
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; } = string.Empty;
        public Guid PartNumberLogisticId { get; set; }
        public Guid CompletePartId { get; set; }
        public string CompletePartName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public Guid MaterialSuplierId { get; set; }
        public string PartNumberLogisticDescription { get; set; } = string.Empty; // Assuming this might be useful
        public string MaterialSupplierDescription { get; set; } = string.Empty; // Assuming this might be useful
    }
}
