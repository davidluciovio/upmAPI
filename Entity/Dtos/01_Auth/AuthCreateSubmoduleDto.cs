using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.Auth
{
    public class AuthCreateSubmoduleDto
    {
        public string Submodule { get; set; } = string.Empty;
        public string CreateBy { get; set; } = string.Empty;
        public Guid ModuleId { get; set; }
    }
}
