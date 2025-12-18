using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.DataProduction.DataProductionLine
{
    public class ProductionLineUpdateDto
    {
        public bool Active { get; set; }
        public string LineDescription { get; set; } = string.Empty;
        public string UpdateBy { get; set; } = string.Empty;
    }
}
