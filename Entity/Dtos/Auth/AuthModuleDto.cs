using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.Auth
{
    public class AuthModuleDto
    {
        public Guid Id { get; set; }

        public string Module { get; set; } = string.Empty;
        public List<AuthSubmoduleDto>? Submodules { get; set; }

    }
}
