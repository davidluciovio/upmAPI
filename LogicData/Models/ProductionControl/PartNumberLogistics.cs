using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models.ProductionControl
{
    public class PartNumberLogistics
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; } = string.Empty;

        public Guid PartNumberId { get; set; }
        public Guid AreaId { get; set; }
        public Guid LocationId { get; set; }

        public float SNP { get; set; }

        public virtual ICollection<ComponentAlert> ComponentAlerts { get; set; } = new List<ComponentAlert>();
        public virtual ICollection<PartNumberStructure> PartNumberEstructures { get; set; } = new List<PartNumberStructure>();
    }
}

