using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upmDomain.DomainDowntime
{
    public class DowntimeDto
    {
        public Guid DowntimeId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string InforCode { get; set; } = string.Empty;
        public string PLCCode { get; set; } = string.Empty;
        public bool IsDirectDowntime { get; set; }
        public bool Programable { get; set; }
    }
}
