using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class DataProductionModelCreateDto
    {
        public string CreateBy { get; set; } = string.Empty;
        public string ModelDescription { get; set; } = string.Empty;
    }
}
