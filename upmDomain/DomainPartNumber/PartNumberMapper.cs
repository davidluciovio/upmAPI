using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upmData.Models;

namespace upmDomain.DomainPartNumber
{
    internal class PartNumberMapper
    {
        public static PartNumberDto Map(PartNumber partNumber)
        {
            return new PartNumberDto
            {
                NetoTime = partNumber.NetoTime,
                ObjectiveTime = partNumber.ObjectiveTime,
                PartNumberConfigurationId = new DomainPartNumberConfiguration.PartNumberConfigurationDto
                {
                    LineId = partNumber.PartNumberConfiguration.LineId,
                    ModelId = partNumber.PartNumberConfiguration.ModelId,
                    PartNumberConfiogurationId = partNumber.PartNumberConfiguration.Id,
                    PartNumberId = partNumber.Id
                },
                PartNumberId = partNumber.Id,
                PartNumberName = partNumber.PartNumberName
            };
        }
    }
}
