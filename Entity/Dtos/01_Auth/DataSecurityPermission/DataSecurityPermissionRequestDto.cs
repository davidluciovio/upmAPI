using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos._01_Auth.DataSecurityPermission
{
    public class DataSecurityPermissionRequestDto
    {
        public bool Active { get; set; }
        public string CreateBy { get; set; } = string.Empty;

        public string Permission { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
        public Guid SubmoduleId { get; set; }
    }
}
