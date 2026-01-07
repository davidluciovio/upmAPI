using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ModelDtos._04_AssyProduction.ProductionRegister
{
    public class ProductionRegisterResponseDto
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; } = string.Empty;

        public float Quantity { get; set; }
        public int Counter { get; set; }

        public string ProductionStation { get; set; } = string.Empty;
    }
}
