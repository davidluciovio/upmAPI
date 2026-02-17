using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models.ProductionControl
{
    public class PartNumberStructure
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; } = string.Empty;


        public string PartNumberName { get; set; } = string.Empty;
        public string PartNumberDescription { get; set; } = string.Empty;

        public Guid ProductionStationId { get; set; }
        
        public Guid PartNumberLogisticsId { get; set; }
        public virtual PartNumberLogistics PartNumberLogistics { get; set; } = new PartNumberLogistics();

        public Guid MaterialSuplierId { get; set; }
        public virtual MaterialSupplier MaterialSupplier { get; set; } = new MaterialSupplier();
    }
}
