using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class AuthCreatePermissionDto
    {
        public string CreateBy { get; set; } = string.Empty;
        public string Permission { get; set; } = string.Empty; // Ej: "Crear Venta"
        public string Clave { get; set; } = string.Empty; // Ej: "Permisos.Ventas.Crear"
        public Guid SubmoduleId { get; set; } // A qué Submódulo padre pertenece
    }
}
