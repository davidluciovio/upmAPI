using Entity.Models.DataProduction;
using Entity.Models.DataUPM;
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

        public DbSet<DataProductionDowntime> ProductionDowntimes { get; set; }
        public DbSet<DataProductionModel> ProductionModels { get; set; }
        public DbSet<DataProductionLocation> ProductionLocations { get; set; }
        public DbSet<DataProductionPartNumber> ProductionPartNumbers { get; set; }
        public DbSet<DataProductionArea> ProductionAreas { get; set; }
        public DbSet<DataProductionLine> ProductionLines { get; set; }
        public DbSet<DataStatus> Statuses { get; set; }
        public DbSet<DataActiveEmployees> ActiveEmployees { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("upm_data");
            base.OnModelCreating(builder);

            builder.Entity<DataProductionDowntime>(entity =>
            {
                entity.ToTable("ProductionDowntime");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.DowntimeDescription).IsUnique();

                entity.Property(e => e.DowntimeDescription).IsRequired();
            });


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

            builder.Entity<DataProductionLine>(entity =>
            {
                entity.ToTable("ProductionLine");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.LineDescription).IsUnique();
                entity.Property(e => e.LineDescription).IsRequired().HasMaxLength(500);
            });

            builder.Entity<DataStatus>(entity =>
            {
                entity.ToTable("DataStatus");
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.StatusDescription).IsUnique();

                entity.Property(e => e.StatusDescription).IsRequired().HasMaxLength(100);
            });

            builder.Entity<DataWorkShift>(entity =>
            {
                entity.ToTable("DataWorkShift");
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


            });

            builder.Entity<DataActiveEmployees>(entity =>
            {
                entity.ToView("vw_active_employees");
                entity.HasNoKey();

                entity.Property(e => e.CB_NOMBRES).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CB_APE_MAT).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CB_APE_PAT).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PRETTYNAME).IsRequired().HasMaxLength(300);

            });
        }
    }
}
