using System;

namespace Entity.Dtos.ModelDtos.ProductionControl.MaterialSupplier
{
    public class MaterialSupplierResponseDto
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; } = string.Empty;
        public string MaterialSupplierDescription { get; set; } = string.Empty;
    }
}
