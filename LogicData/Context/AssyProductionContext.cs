using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicData.Context
{
    public class AssyProductionContext : DbContext
    {
        public AssyProductionContext()
        {
            
        }

        public AssyProductionContext(DbContextOptions<AssyProductionContext> options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("upm_assyProduction");
            base.OnModelCreating(builder);
        }
    }
}
