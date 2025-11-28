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

        public DbSet<Entity.Models.AssyProduction.ProductionStation> ProductionStations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("upm_assyProduction");
            base.OnModelCreating(builder);

            builder.Entity<Entity.Models.AssyProduction.ProductionStation>(entity =>
            {
                entity.ToTable("ProductionStation");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.PartNumberId);
                entity.HasIndex(e => e.LineId);

                entity.Property(e => e.PartNumberId).IsRequired();
                entity.Property(e => e.LineId).IsRequired();
            });
        }
    }
}
