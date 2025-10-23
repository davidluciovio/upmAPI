using Entity.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class AuthSubmodule
    {
        public Guid Id { get; set; }

        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;

        public string Submodule { get; set; } = string.Empty;
        public Guid ModuleId { get; set; }

        public AuthModule AuthModule { get; set; } = new AuthModule();

        public virtual ICollection<AuthPermissions> AuthPermissions { get; set; } = new List<AuthPermissions>();

    }
}
