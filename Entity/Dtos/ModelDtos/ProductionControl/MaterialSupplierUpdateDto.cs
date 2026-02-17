using System;

namespace Entity.ModelDtos.ProductionControl
{
    public class MaterialSupplierUpdateDto
    {
        public bool Active { get; set; }
        public string UpdateBy { get; set; } = string.Empty;
        public string MaterialSupplierDescription { get; set; } = string.Empty;
    }
}
