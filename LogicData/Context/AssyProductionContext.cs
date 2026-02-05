using Entity.Models._04_AssyProduction;
using Entity.Models.AssyProduction;
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

        public DbSet<ProductionStation> ProductionStations { get; set; }
        public DbSet<ProductionRegister> ProductionRegisters { get; set; }
        public DbSet<DowntimeRegister> DowntimeRegisters { get; set; }
        public DbSet<LineOperatorsRegister> LineOperatorsRegisters { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("upm_assyProduction");
            base.OnModelCreating(builder);

            builder.Entity<ProductionStation>(entity =>
            {
                entity.ToTable("ProductionStation");
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.PartNumberId);
                entity.HasIndex(e => e.LineId);
                entity.HasIndex(e => e.ModelId);

                entity.Property(e => e.PartNumberId).IsRequired();
                entity.Property(e => e.LineId).IsRequired();
                entity.Property(e => e.ModelId).IsRequired();
            });

            builder.Entity<ProductionRegister>(entity =>
            {
                entity.ToTable("ProductionRegister");
                entity.HasKey(e => e.Id);
                
                entity.HasOne(e => e.ProductionStation).WithMany(ps => ps.PrductionRegisters).HasForeignKey(e => e.ProductionStationId);

            });

            builder.Entity<DowntimeRegister>(entity =>
            {
                entity.ToTable("DowntimeRegister");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.ProductionStation).WithMany(ps => ps.DowntimeRegisters).HasForeignKey(e => e.ProductionStationId);
            });

            builder.Entity<LineOperatorsRegister>(entity =>
            {
                entity.ToTable("LineOperatorsRegister");
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.LineId);
                entity.Property(e => e.LineId).IsRequired();

            });
        }
    }
}
