using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upmData.Context;
using upmDomain.Lider;

namespace upmDomain.LineDomain
{
    public class LineService
    {
        private readonly UpmwebContext _context;
        public LineService(UpmwebContext context)
        {
            _context = context;
        }

        public async Task<List<LineDto>> GetByLidersAsync(List<Guid> liders)
        {
            var lines = await _context.LiderConfigurations
                .Where(lc => lc.Active && liders.Contains(lc.UserId))
                .Select(lc => LineMapper.Map(lc.PartNumberConfiguration.Line))
                .Distinct()
                .ToListAsync();

            return lines;
        }
    }
}
