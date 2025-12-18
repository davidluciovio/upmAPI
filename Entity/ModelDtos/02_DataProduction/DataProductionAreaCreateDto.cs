using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.DataProduction
{
    public class DataProductionAreaCreateDto
    {
        public string CreateBy { get; set; } = string.Empty;
        public string AreaDescription { get; set; } = string.Empty;
    }
}
