using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models.DataProduction
{
    public class DataProductionPartNumber
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;

        public string PartNumberName { get; set; } = string.Empty;
        public string PartNumberDescription { get; set; } = string.Empty;
        public string SNP { get; set; } = string.Empty;

        public virtual DataProductionModel? DataProductionModel { get; set; }
        public Guid ProductionModelId { get; set; }
        public virtual DataProductionLocation? DataProductionLocation { get; set; }
        public Guid ProductionLocationId { get; set; }
        public virtual DataProductionArea? DataProductionArea { get; set; }
        public Guid ProductionAreaId { get; set; }


    }
}
