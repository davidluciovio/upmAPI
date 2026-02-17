using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.ModelDtos.ProductionControl.ProductionLocation
{
    public class PartNumberLocationCreateDto
    {
        public Guid PartNumberId { get; set; }
        public Guid LocationId { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public string UpdateBy { get; set; } = string.Empty;
    }
}
