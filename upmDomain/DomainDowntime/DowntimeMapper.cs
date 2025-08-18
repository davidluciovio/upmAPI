using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upmData.Models;

namespace upmDomain.DomainDowntime
{
    internal class DowntimeMapper
    { 
        public static DowntimeDto Map(Downtime downtime)
        {
            return new DowntimeDto
            {
                Description = downtime.Description,
                DowntimeId = downtime.Id,
                InforCode = downtime.InforCode,
                IsDirectDowntime = downtime.IsDirectDowntime,
                PLCCode = downtime.Plccode,
                Programable = downtime.Programable
            };
        }
    }
}
