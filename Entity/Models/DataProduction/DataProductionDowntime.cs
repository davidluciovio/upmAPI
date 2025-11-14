using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models.DataProduction
{
    public class DataProductionDowntime
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;

        public string DowntimeDescription { get; set; } = string.Empty;
        public string InforCode { get; set; } = string.Empty;
        public string PLCCode { get; set; } = string.Empty;
        public bool IsDirectDowntime { get; set; }
        public bool Programable { get; set; }
    }
}
