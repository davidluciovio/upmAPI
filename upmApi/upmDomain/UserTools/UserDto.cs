using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upmDomain.UserTools
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public bool Active { get; set; }
        public string CodeUser { get; set; } 
        public string CreateBy { get; set; } 
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? Password { get; set; }

    }
}
