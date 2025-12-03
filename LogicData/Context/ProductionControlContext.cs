using Entity.Models.DataProduction;
using Entity.Models.ProductionControl;
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

        //public DbSet<ProductionControlComponentAlert> ComponentAlerts { get; set; }
        public DbSet<PartNumberLogistics> partNumberLogistics { get; set; }
        public DbSet<PartNumberLocation> PartNumberLocations { get; set; }


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
            //builder.Entity<ProductionControlComponentAlert>()
            //    .HasOne<DataProductionPartNumber>()
            //    .WithMany()
            //    .HasForeignKey(e => e.ProductionPartNumberId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<ProductionControlComponentAlert>(entity =>
            //{
            //    entity.ToTable("ComponentAlert");
            //    entity.HasKey(e => e.Id);

            //});

        }
            
    }
}
