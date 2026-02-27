using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models.AssyProduction
{
    public class LineOperatorsRegister
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; } = string.Empty;

        public Guid LineId { get; set; }
        public string OperatorCode { get; set; } = string.Empty;
        public string OperatorName { get; set; } = string.Empty;

        public DateTime StartDatetime { get; set; }
        public DateTime EndDatetime { get; set; }

        public bool? Complete { get; set; }

    }
}
