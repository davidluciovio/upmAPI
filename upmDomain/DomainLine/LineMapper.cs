using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upmData.Models;

namespace upmDomain.LineDomain
{
    internal class LineMapper
    {
        public static LineDto Map(Line line)
        {
            return new LineDto
            {
                LineId = line.Id,
                LineCode = line.CodeLine,
                LineName = line.LineName,
                WorkCenter = line.WorkCenter,
            };
        }
    }
}
