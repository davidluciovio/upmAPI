using Entity.Models._05_Temporal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicData.Context
{
    public class TemporalContext : DbContext
    {
        public TemporalContext()
        {
            
        }

        public TemporalContext(DbContextOptions<TemporalContext> options) : base(options)
        {

        }

        public DbSet<ProductionAchievement> ProductionAchievements { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("upm_temporal");
            base.OnModelCreating(builder);

            builder.Entity<ProductionAchievement>(entity =>
            {
                entity.ToTable("ProductionAchievement");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");

                entity.Property(e => e.Supervisor).HasMaxLength(100);
                entity.Property(e => e.Leader).HasMaxLength(100);
                entity.Property(e => e.Shift).HasMaxLength(50);
                entity.Property(e => e.PartNumberName).HasMaxLength(200);
            });
        }
    }
}
