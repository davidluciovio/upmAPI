using Entity.Models.DataProduction;
using Entity.Models.ProductionControl;
using LogicData.Models.ProductionControl;
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

        public DbSet<ComponentAlert> ComponentAlerts { get; set; }
        public DbSet<PartNumberLogistics> PartNumberLogistics { get; set; }
        public DbSet<PartNumberLocation> PartNumberLocations { get; set; }
        public DbSet<MaterialSupplier> MaterialSuppliers { get; set; }
        public DbSet<PartNumberStructure> PartNumberStructures { get; set; }
        public DbSet<ForkliftArea> ForkliftAreas { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("upm_productionControl");
            base.OnModelCreating(builder);

            builder.Entity<PartNumberLogistics>(entity =>
            {
                entity.ToTable("PartNumberLogistics");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.PartNumberId);
                entity.HasIndex(e => e.AreaId);
                entity.HasIndex(e => e.LocationId);

                entity.Property(e => e.PartNumberId).IsRequired();
                entity.Property(e => e.AreaId).IsRequired();
                entity.Property(e => e.LocationId).IsRequired();

                entity.Property(e => e.SNP).HasDefaultValue(0);
            });

            builder.Entity<PartNumberLocation>(entity =>
            {
                entity.ToTable("PartNumberLocation");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.PartNumberId);
                entity.HasIndex(e => e.LocationId);

                entity.Property(e => e.PartNumberId).IsRequired();
                entity.Property(e => e.LocationId).IsRequired();
            });

            builder.Entity<ComponentAlert>(entity =>
            {
                entity.ToTable("ComponentAlert");
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.StatusId);

                entity.Property(e => e.StatusId).IsRequired();

                entity.HasOne(e => e.PartNumberLogistics)
                      .WithMany(i => i.ComponentAlerts)
                      .HasForeignKey(e => e.PartNumberLogisticsId);

            });

            builder.Entity<MaterialSupplier>(entity =>
            {
                entity.ToTable("MaterialSuplier");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MaterialSupplierDescription).IsRequired().HasMaxLength(250);
            });

            builder.Entity<PartNumberStructure>(entity =>
            {
                entity.ToTable("PartNumberStructure");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.ProductionStationId);

                entity.Property(e => e.ProductionStationId).IsRequired();

                entity.Property(e => e.PartNumberName).IsRequired().HasMaxLength(250);
                entity.Property(e => e.PartNumberDescription).IsRequired();

                entity.HasOne(e => e.PartNumberLogistics)
                      .WithMany(pl => pl.PartNumberStructures)
                      .HasForeignKey(e => e.PartNumberLogisticsId);

                entity.HasOne(e => e.MaterialSupplier)
                      .WithMany(ms => ms.PartNumberStructures)
                      .HasForeignKey(e => e.MaterialSuplierId);
            });

            builder.Entity<ForkliftArea>(entity =>
            {
                entity.ToTable("ForkliftArea");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.DataProductionAreaId);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.DataProductionAreaId).IsRequired();

            });
        }
    }
}
