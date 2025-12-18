using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.DataProduction.DataProductionLine
{
    public class ProductionLineCreateDto
    {
        public string CreateBy { get; set; } = string.Empty;
        public string LineDescription { get; set; } = string.Empty;
    }
}
