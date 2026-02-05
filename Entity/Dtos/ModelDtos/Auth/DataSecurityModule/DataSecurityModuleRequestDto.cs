using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos._00_DataUPM.DataSecurityModule
{
    public class DataSecurityModuleRequestDto
    {

        public bool Active { get; set; }
        public string CreateBy { get; set; } = string.Empty;

        public string Module { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
    }
}
