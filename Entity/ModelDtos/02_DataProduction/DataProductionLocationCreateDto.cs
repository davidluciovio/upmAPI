using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.DataProduction
{
    public class DataProductionLocationCreateDto
    {
        public string CreateBy { get; set; } = string.Empty;
        public string LocationDescription { get; set; } = string.Empty;
    }
}

