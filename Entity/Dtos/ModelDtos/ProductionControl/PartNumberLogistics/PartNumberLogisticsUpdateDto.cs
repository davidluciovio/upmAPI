using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.ProductionControl
{
    public class PartNumberLogisticsUpdateDto
    {
        public Guid PartNumberId { get; set; }
        public Guid AreaId { get; set; }
        public Guid LocationId { get; set; }
        public float SNP { get; set; }
        public bool Active { get; set; }
        public string UpdateBy { get; set; } = string.Empty;
        public string CreateBy { get; set; } = string.Empty;
    }
}
