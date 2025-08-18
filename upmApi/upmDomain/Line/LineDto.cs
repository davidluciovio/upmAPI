using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upmDomain.LineDomain
{
    public class LineDto
    {
        public Guid LineId { get; set; }

        public string LineName { get; set; }
        public string LineCode { get; set; }
        public string WorkCenter { get; set; }
    }
}
