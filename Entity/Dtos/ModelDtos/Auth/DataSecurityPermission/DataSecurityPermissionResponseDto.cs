
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos._01_Auth.DataSecurityPermission
{
    public class DataSecurityPermissionResponseDto
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }

        public string Permission { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
        public Guid SubmoduleId { get; set; }

    }
}
