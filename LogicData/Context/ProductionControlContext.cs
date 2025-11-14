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

        public DbSet<ProductionControlComponentAlert> ComponentAlerts { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("upm_pc");
            base.OnModelCreating(builder);

            builder.Ignore<DataProductionModel>();
            builder.Ignore<DataProductionLocation>();
            builder.Ignore<DataProductionPartNumber>();

            builder.Entity<ProductionControlComponentAlert>()
                .HasOne<DataProductionPartNumber>()
                .WithMany()
                .HasForeignKey(e => e.ProductionPartNumberId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProductionControlComponentAlert>(entity =>
            {
                entity.ToTable("ComponentAlert");
                entity.HasKey(e => e.Id);

            });

        }
            
    }
}
