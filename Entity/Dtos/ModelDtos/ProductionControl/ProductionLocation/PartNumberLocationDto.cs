using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.ModelDtos.ProductionControl.ProductionLocation
{
    public class PartNumberLocationDto
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; } = string.Empty;

        public string PartNumber { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}
