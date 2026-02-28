using Entity.Dtos.AssyProduction;
using Entity.Dtos.ProductionControl;
using System;

namespace Entity.Dtos.ModelDtos.ProductionControl.PartNumberStructure
{
    public class PartNumberStructureResponseDto
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;

        public Guid ProductionStationId { get; set; }

        public ProductionStationResponseDto ProductionStation { get; set; } = new ProductionStationResponseDto();
        public string PartNumberDescription { get; set; } = string.Empty;

        public Guid PartNumberLogisticId { get; set; }
        public PartNumberLogisticsResponseDto PartNumberLogistic { get; set; } = new PartNumberLogisticsResponseDto();

        public string MaterialSupplierDescription { get; set; } = string.Empty; // Assuming this might be useful
    }
}
