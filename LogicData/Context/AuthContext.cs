using Entity.Dtos.Auth;
using Entity.Models.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicData.Context
{
    public class AuthContext : IdentityDbContext<AuthUser, AuthRole, string>
    {
        public AuthContext() { }

        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {

        }

        public DbSet<AuthModule> Modules { get; set; }
        public DbSet<AuthSubmodule> Submodulos { get; set; }
        public DbSet<AuthPermissions> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("upm_auth");
            base.OnModelCreating(builder);

            builder.Entity<AuthModule>(entity =>
            {
                entity.ToTable("Modules", "upm_auth");
                entity.HasKey(e => e.Id);
                
                entity.HasIndex(e => e.Module).IsUnique();

                entity.Property(e => e.Module).IsRequired().HasMaxLength(100);

                entity.HasMany(e => e.AuthSubmodules)
                      .WithOne(e => e.AuthModule)
                      .HasForeignKey(e => e.ModuleId);
            });

            builder.Entity<AuthSubmodule>(entity =>
            {
                entity.ToTable("Submodules", "upm_auth");
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Submodule).IsUnique();

                entity.Property(e => e.Submodule).IsRequired().HasMaxLength(100);

                entity.HasMany(e => e.AuthPermissions)
                      .WithOne(e => e.AuthSubmodule)
                      .HasForeignKey(e => e.SubmoduleId);
            });

            builder.Entity<AuthPermissions>(entity =>
            {
                entity.ToTable("Permissions", "upm_auth");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Clave).IsUnique();
                entity.Property(e => e.Permission).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Clave).IsRequired().HasMaxLength(200);

            });


            //Seed initial data if needed


        }
    }
}
