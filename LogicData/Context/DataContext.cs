using Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicData.Context
{
    public class DataContext : DbContext
    {
        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<DataProductionModel> ProductionModels { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("upm");
            base.OnModelCreating(builder);

            builder.Entity<DataProductionModel>(entity =>
            {
                entity.ToTable("ProductionModel");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.ProductionModelName).IsUnique();

                entity.Property(e => e.ProductionModelName).IsRequired().HasMaxLength(200);
            });

            builder.Entity<DataProductionLocation>(entity =>
            {
                entity.ToTable("ProductionLocation");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.ProductionLocationName).IsUnique();

                entity.Property(e => e.ProductionLocationName).IsRequired().HasMaxLength(200);
            });

            builder.Entity<DataProductionPartNumber>(entity =>
            {
                entity.ToTable("ProductionPartNumber");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.ProductionPartNumberName).IsUnique();

                entity.Property(e => e.ProductionPartNumberName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ProductionPartNumberDescription).HasMaxLength(500);

                entity.HasOne(e => e.DataProductionModel)
                      .WithMany(e => e.ProductionPartNumbers)
                      .HasForeignKey(e => e.ProductionModelId);

                entity.HasOne(e => e.DataProductionLocation)
                      .WithMany(e => e.ProductionPartNumbers)
                      .HasForeignKey(e => e.ProductionLocationId);
            });
        }
    }
}
