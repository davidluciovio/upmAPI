using Entity.Dtos.ProductionControl;
using System;

namespace Entity.Dtos.ModelDtos.ProductionControl.PartNumberEstructure
{
    public class PartNumberEstructureResponseDto
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;

        public PartNumberLogisticsResponseDto? PartNumberLogisticsResponse { get; set; }

        public string CompletePartName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string? MaterialSupplierDescription { get; set; } 
    }
}
