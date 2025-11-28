using Entity.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.Auth
{
    public class AuthPermissions
    {
        public Guid Id { get; set; }

        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;

        public string Permission { get; set; } = string.Empty; // Ej: "Crear Factura"

        // ESTA ES LA CLAVE: Una cadena única para la lógica de autorización
        public string Clave { get; set; } = string.Empty; // Ej: "Permisos.Ventas.Facturacion.Crear"
        public Guid SubmoduleId { get; set; }

        public AuthSubmodule AuthSubmodule { get; set; } = new AuthSubmodule();
    }
}
