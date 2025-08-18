using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upmDomain.Lider
{
    public class LiderDto
    {
        public Guid LiderId { get; set; }
        public string LiderName { get; set; } = string.Empty;
        public string LiderCode { get; set; } = string.Empty;
    }
}
