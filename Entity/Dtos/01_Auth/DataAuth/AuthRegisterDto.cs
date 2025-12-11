using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos._01_Auth.DataAuth
{
    public class AuthRegisterDto
    {
        public string Email { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string Password { get; set; } = string.Empty;
        public string CreateBy { get; set; } = string.Empty;
    }
}
