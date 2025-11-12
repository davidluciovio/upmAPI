using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class DataProductionPartNumberCreateDto
    {
        public string CreateBy { get; set; } = string.Empty;
        public string PartNumberName { get; set; } = string.Empty;
        public string PartNumberDescription { get; set; } = string.Empty;
        public string SNP { get; set; } = string.Empty;

        public Guid ProductionModelId { get; set; }
        public Guid ProductionLocationId { get; set; }
        public Guid ProductionAreaId { get; set; }
    }
}
