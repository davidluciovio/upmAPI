using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class DataProductionPartNumberDto
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;

        public string PartNumberName { get; set; } = string.Empty;
        public string PartNumberDescription { get; set; } = string.Empty;
        public string SNP { get; set; } = string.Empty;

        public string? ProductionModel { get; set; }
        public string? ProductionLocation { get; set; }
        public string? ProductionArea { get; set; }
    }
}
