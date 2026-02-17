using System;

namespace Entity.ModelDtos.ProductionControl
{
    public class MaterialSupplierCreateDto
    {
        public bool Active { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public string MaterialSupplierDescription { get; set; } = string.Empty;
    }
}
