using Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicData.Context
{
    public class ProductionControlContext : DbContext
    {
        public ProductionControlContext() { }
        public ProductionControlContext(DbContextOptions<ProductionControlContext> options) : base(options)
        {
        }

        public DbSet<ProductionControlStatus> statuses { get; set; }
        public DbSet<ProductionControlComponentAlert> ComponentAlerts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("upm_pc");
            base.OnModelCreating(builder);

            builder.Entity<ProductionControlStatus>(entity =>
            {
                entity.ToTable("Status");
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.StatusDescription).IsUnique();

                entity.Property(e => e.StatusDescription).IsRequired().HasMaxLength(100);
            });

            builder.Entity<DataProductionPartNumber>()
                .ToTable("ProductionPartNumber", t => t.ExcludeFromMigrations());

            builder.Entity<ProductionControlComponentAlert>(entity =>
            {
                entity.ToTable("ComponentAlert");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Status)
                    .WithMany(e => e.ComponentsAlerts)
                    .HasForeignKey(e => e.StatusId);
                entity.HasOne(e => e.ProductionPartNumber)
                    .WithMany(e => e.ComponentsAlerts)
                    .HasForeignKey(e => e.ProductionPartNumberId);
            });

            builder.Entity<ProductionControlArea>(entity =>
            {
                entity.ToTable("Area");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.AreaName).IsUnique();
                entity.Property(e => e.AreaName).IsRequired().HasMaxLength(100);
            });
        }   
    }
}
