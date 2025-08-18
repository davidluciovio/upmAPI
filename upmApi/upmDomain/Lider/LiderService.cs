using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upmData.Context;
using upmDomain.Interfaces;
using upmDomain.UserTools;

namespace upmDomain.Lider
{
    public class LiderService : IService<LiderDto>
    {
        private readonly UpmwebContext _context;
        public LiderService(UpmwebContext context) 
        {
            _context = context;
        }

        public async Task<List<LiderDto>> GetAllAsync()
        {
            var liders = await _context.UserConfigurations
                .Where(uc => uc.RoleId == Guid.Parse("4352CAEC-50B1-48EA-9308-E7BBEEC61800"))
                .Select(uc => new LiderDto
                {
                    LiderCode = uc.User.CodeUser,
                    LiderId = uc.User.Id,
                    LiderName = uc.User.UserName
                }).ToListAsync();

            return liders;
        }
    }
}
