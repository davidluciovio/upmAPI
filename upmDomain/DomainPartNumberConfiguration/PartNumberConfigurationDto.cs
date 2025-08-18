using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upmDomain.DomainPartNumberConfiguration
{
    public class PartNumberConfigurationDto
    {
        public int PartNumberConfiogurationId { get; set; }
        public Guid PartNumberId { get; set; }
        public Guid LineId { get; set; }
        public int ModelId { get; set; }
    }
}
