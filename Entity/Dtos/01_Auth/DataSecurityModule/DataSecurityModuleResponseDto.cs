using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos._00_DataUPM.DataSecurityModule
{
    public class DataSecurityModuleResponseDto
    {
        public Guid Id { get; set; }

        public bool Active { get; set; }
        
        public string Module { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
    }
}
