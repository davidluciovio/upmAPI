using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upmDomain.DomainPartNumberConfiguration;

namespace upmDomain.DomainPartNumber
{
    public class PartNumberDto
    {
        public Guid PartNumberId { get; set; }
        public string? PartNumberName { get; set; }
        public float? ObjectiveTime { get; set; }
        public float? NetoTime { get; set; }

        public PartNumberConfigurationDto? PartNumberConfigurationId { get; set; }
    }
}
