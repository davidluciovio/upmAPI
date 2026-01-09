using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models.Auth
{
    public class AuthPermissions
    {
        public Guid Id { get; set; }

        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;

        public string Permission { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty; 
        public Guid SubmoduleId { get; set; }

        public virtual AuthSubmodule AuthSubmodule { get; set; } = new AuthSubmodule();
    }
}
