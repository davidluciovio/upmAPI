namespace Entity.Dtos.ModelDtos.ProductionControl.MaterialSupplier
{
    public class MaterialSupplierRequestDto
    {
        public string MaterialSupplierDescription { get; set; } = string.Empty;
        public string CreateBy { get; set; } = string.Empty;
        public string UpdateBy { get; set; } = string.Empty;
        public bool Active { get; set; }
    }
}
