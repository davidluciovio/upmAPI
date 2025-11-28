using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.Auth
{
    public class AuthCreateModuloDto
    {
        public string Module { get; set; } = string.Empty;
        public string CreateBy { get; set; } = string.Empty;
    }
}
