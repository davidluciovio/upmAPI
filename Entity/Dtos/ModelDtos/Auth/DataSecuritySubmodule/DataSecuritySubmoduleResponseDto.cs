using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos._01_Auth.DataSecuritySubmodule
{
    public class DataSecuritySubmoduleResponseDto
    {
        public Guid Id { get; set; }

        public bool Active { get; set; }
        

        public string Submodule { get; set; } = string.Empty;
        public Guid ModuleId { get; set; }
         
        public string Icon { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
    }
}
