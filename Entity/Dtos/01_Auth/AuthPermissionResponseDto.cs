using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.Auth
{
    public class AuthPermissionResponseDto
    {
        public Guid Id { get; set; }
        public string Permission { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
        public Guid SubmoduleId { get; set; }
    }
}
