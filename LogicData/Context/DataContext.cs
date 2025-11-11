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
        public DbSet<DataProductionLocation> ProductionLocations { get; set; }
        public DbSet<DataProductionPartNumber> ProductionPartNumbers { get; set; }
        public DbSet<DataProductionArea> ProductionAreas { get; set; }
        public DbSet<DataStatus> Statuses { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("upm");
            base.OnModelCreating(builder);

            builder.Entity<DataProductionModel>(entity =>
            {
                entity.ToTable("ProductionModel");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.ModelDescription).IsUnique();

                entity.Property(e => e.ModelDescription).IsRequired().HasMaxLength(200);
            });

            builder.Entity<DataProductionLocation>(entity =>
            {
                entity.ToTable("ProductionLocation");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.LocationDescription).IsUnique();

                entity.Property(e => e.LocationDescription).IsRequired().HasMaxLength(200);
            }); 
            
            builder.Entity<DataProductionArea>(entity =>
            {
                entity.ToTable("ProductionArea");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.AreaDescription).IsUnique();
                entity.Property(e => e.AreaDescription).IsRequired().HasMaxLength(100);
            });

            builder.Entity<DataStatus>(entity =>
            {
                entity.ToTable("Status");
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.StatusDescription).IsUnique();

                entity.Property(e => e.StatusDescription).IsRequired().HasMaxLength(100);
            });

            builder.Entity<DataWorkShift>(entity =>
            {
                entity.ToTable("WorkShift");
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.ShiftDescription).IsUnique();

                entity.Property(e => e.ShiftDescription).IsRequired().HasMaxLength(100);
            });

            builder.Entity<DataProductionPartNumber>(entity =>
            {
                entity.ToTable("ProductionPartNumber");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.PartNumberName).IsUnique();

                entity.Property(e => e.PartNumberName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.PartNumberDescription).HasMaxLength(500);

                entity.HasOne(e => e.DataProductionModel)
                      .WithMany(e => e.ProductionPartNumbers)
                      .HasForeignKey(e => e.ProductionModelId);

                entity.HasOne(e => e.DataProductionLocation)
                      .WithMany(e => e.ProductionPartNumbers)
                      .HasForeignKey(e => e.ProductionLocationId);

                entity.HasOne(e => e.DataProductionArea)
                      .WithMany(e => e.ProductionPartNumbers)
                      .HasForeignKey(e => e.ProductionAreaId);
            });

        }
    
    }
}
