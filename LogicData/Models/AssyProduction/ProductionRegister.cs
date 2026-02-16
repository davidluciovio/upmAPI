using Entity.Models.AssyProduction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models._04_AssyProduction
{
    public class ProductionRegister
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; } = string.Empty;

        public float Quantity { get; set; }
        public int Counter { get; set; }

        public Guid ProductionStationId { get; set; }

        public virtual ProductionStation ProductionStation { get; set; } = new ProductionStation();
    }
}
