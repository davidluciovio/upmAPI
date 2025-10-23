using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class AuthLoginDto
    {
        public string CodeUser { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
