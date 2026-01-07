using Entity.Models.AssyProduction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ModelDtos._04_AssyProduction.ProductionRegister
{
    public class ProductionRegisterRequestDto
    {
        public bool Active { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; } = string.Empty;

        public float Quantity { get; set; }
        public int Counter { get; set; }

        public Guid ProductionStationId { get; set; }

    }
}
