using Entity.Models._04_AssyProduction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models.AssyProduction
{
    public class ProductionStation
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; } = string.Empty;

        public Guid PartNumberId { get; set; }
        public Guid LineId { get; set; }
        public Guid ModelId { get; set; }

        public float ObjetiveTime { get; set; }
        public float NetoTime { get; set; }
        public int OperatorQuantity { get; set; }
        public int PartNumberQuantity { get; set; }

        public virtual ICollection<ProductionRegister> PrductionRegisters { get; set; } = new List<ProductionRegister>();
        public virtual ICollection<DowntimeRegister> DowntimeRegisters { get; set; } = new List<DowntimeRegister>();
    }
}
