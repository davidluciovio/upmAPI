using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class DataProductionPartNumberCreateDto
    {
        public string CreatedBy { get; set; } = string.Empty;
        public string PartNumberName { get; set; } = string.Empty;
        public string PartNumberDescription { get; set; } = string.Empty;
        public string SNP { get; set; } = string.Empty;
    }
}
