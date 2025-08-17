using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using upmData.Models;

namespace upmData.Context;

public partial class UpmwebContext : DbContext
{
    public UpmwebContext()
    {
    }

    public UpmwebContext(DbContextOptions<UpmwebContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Downtime> Downtimes { get; set; }

    public virtual DbSet<DowntimeRegister> DowntimeRegisters { get; set; }

    public virtual DbSet<DowntimeRegisterResponsable> DowntimeRegisterResponsables { get; set; }

    public virtual DbSet<EmployeeReport> EmployeeReports { get; set; }

    public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

    public virtual DbSet<LiderConfiguration> LiderConfigurations { get; set; }

    public virtual DbSet<Line> Lines { get; set; }

    public virtual DbSet<MaterialAlert> MaterialAlerts { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<PartNumber> PartNumbers { get; set; }

    public virtual DbSet<PartNumberConfiguration> PartNumberConfigurations { get; set; }

    public virtual DbSet<ProductionRegister> ProductionRegisters { get; set; }

    public virtual DbSet<RackRegister> RackRegisters { get; set; }

    public virtual DbSet<RequerimentsHistory> RequerimentsHistories { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SupplyArea> SupplyAreas { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserConfiguration> UserConfigurations { get; set; }

    public virtual DbSet<ViewActiveColaboradore> ViewActiveColaboradores { get; set; }

    public virtual DbSet<ViewComponentsTree> ViewComponentsTrees { get; set; }

    public virtual DbSet<WorkShift> WorkShifts { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Server=(local);Database=UPMWEB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Modern_Spanish_CI_AS");

        modelBuilder.Entity<Downtime>(entity =>
        {
            entity.ToTable("Downtime", "upmdat");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Plccode).HasColumnName("PLCCode");
        });

        modelBuilder.Entity<DowntimeRegister>(entity =>
        {
            entity.ToTable("DowntimeRegister", "upmap");

            entity.HasIndex(e => e.DowntimeId, "IX_DowntimeRegister_DowntimeId");

            entity.HasIndex(e => e.PartNumberConfigurationId, "IX_DowntimeRegister_PartNumberConfigurationId");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.Downtime).WithMany(p => p.DowntimeRegisters).HasForeignKey(d => d.DowntimeId);

            entity.HasOne(d => d.PartNumberConfiguration).WithMany(p => p.DowntimeRegisters).HasForeignKey(d => d.PartNumberConfigurationId);
        });

        modelBuilder.Entity<DowntimeRegisterResponsable>(entity =>
        {
            entity.ToTable("DowntimeRegister_Responsables", "upmap");

            entity.HasIndex(e => e.DowntimeRegisterId, "IX_DowntimeRegister_Responsables_DowntimeRegisterId");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.DowntimeRegister).WithMany(p => p.DowntimeRegisterResponsables).HasForeignKey(d => d.DowntimeRegisterId);
        });

        modelBuilder.Entity<EmployeeReport>(entity =>
        {
            entity.ToTable("EmployeeReport", "upmap");

            entity.HasIndex(e => e.LineId, "IX_EmployeeReport_LineId");

            entity.HasIndex(e => e.WorkShiftId, "IX_EmployeeReport_WorkShiftId");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.Line).WithMany(p => p.EmployeeReports).HasForeignKey(d => d.LineId);

            entity.HasOne(d => d.WorkShift).WithMany(p => p.EmployeeReports).HasForeignKey(d => d.WorkShiftId);
        });

        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.ToTable("ErrorLog", "upmdat");
        });

        modelBuilder.Entity<LiderConfiguration>(entity =>
        {
            entity.ToTable("LiderConfiguration", "upmconf");

            entity.HasIndex(e => e.PartNumberConfigurationId, "IX_LiderConfiguration_PartNumberConfigurationId");

            entity.HasIndex(e => e.UserId, "IX_LiderConfiguration_UserId");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.PartNumberConfiguration).WithMany(p => p.LiderConfigurations).HasForeignKey(d => d.PartNumberConfigurationId);

            entity.HasOne(d => d.User).WithMany(p => p.LiderConfigurations).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Line>(entity =>
        {
            entity.ToTable("Line", "upmdat");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CodeLine).HasDefaultValue("");
            entity.Property(e => e.PlcIp).HasColumnName("PLC_IP");
        });

        modelBuilder.Entity<MaterialAlert>(entity =>
        {
            entity.ToTable("MaterialAlerts", "upmsh");

            entity.HasIndex(e => e.PartNumberConfigurationId, "IX_MaterialAlerts_PartNumberConfigurationId");

            entity.HasOne(d => d.PartNumberConfiguration).WithMany(p => p.MaterialAlerts).HasForeignKey(d => d.PartNumberConfigurationId);
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.ToTable("Model", "upmdat");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<PartNumber>(entity =>
        {
            entity.ToTable("PartNumber", "upmdat");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<PartNumberConfiguration>(entity =>
        {
            entity.ToTable("PartNumberConfiguration", "upmconf");

            entity.HasIndex(e => e.LineId, "IX_PartNumberConfiguration_LineId");

            entity.HasIndex(e => e.ModelId, "IX_PartNumberConfiguration_ModelId");

            entity.HasIndex(e => e.PartNumberId, "IX_PartNumberConfiguration_PartNumberId").IsUnique();

            entity.HasIndex(e => e.SupplyAreaId, "IX_PartNumberConfiguration_SupplyAreaId");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.SupplyAreaId).HasDefaultValue(new Guid("01d1daf8-ab61-42f6-8520-cb1afe9807cd"));

            entity.HasOne(d => d.Line).WithMany(p => p.PartNumberConfigurations).HasForeignKey(d => d.LineId);

            entity.HasOne(d => d.Model).WithMany(p => p.PartNumberConfigurations).HasForeignKey(d => d.ModelId);

            entity.HasOne(d => d.PartNumber).WithOne(p => p.PartNumberConfiguration).HasForeignKey<PartNumberConfiguration>(d => d.PartNumberId);

            entity.HasOne(d => d.SupplyArea).WithMany(p => p.PartNumberConfigurations).HasForeignKey(d => d.SupplyAreaId);
        });

        modelBuilder.Entity<ProductionRegister>(entity =>
        {
            entity.ToTable("ProductionRegister", "upmap");

            entity.HasIndex(e => e.PartNumberConfigurationId, "IX_ProductionRegister_PartNumberConfigurationId");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.PartNumberConfiguration).WithMany(p => p.ProductionRegisters).HasForeignKey(d => d.PartNumberConfigurationId);
        });

        modelBuilder.Entity<RackRegister>(entity =>
        {
            entity.ToTable("RackRegister", "upmap");

            entity.HasIndex(e => e.PartNumberConfigurationId, "IX_RackRegister_PartNumberConfigurationId");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.NoRack).HasColumnName("NoRACK");

            entity.HasOne(d => d.PartNumberConfiguration).WithMany(p => p.RackRegisters).HasForeignKey(d => d.PartNumberConfigurationId);
        });

        modelBuilder.Entity<RequerimentsHistory>(entity =>
        {
            entity.ToTable("RequerimentsHistory", "upmpc");

            entity.HasIndex(e => e.PartNumberConfigurationId, "IX_RequerimentsHistory_PartNumberConfigurationId");

            entity.HasOne(d => d.PartNumberConfiguration).WithMany(p => p.RequerimentsHistories).HasForeignKey(d => d.PartNumberConfigurationId);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role", "upmdat");

            entity.HasIndex(e => e.Description, "IX_Role_Description").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
        });

        modelBuilder.Entity<SupplyArea>(entity =>
        {
            entity.ToTable("SupplyArea", "upmdat");

            entity.HasIndex(e => e.Description, "IX_SupplyArea_Description").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User", "upmdat");

            entity.HasIndex(e => e.CodeUser, "IX_User_CodeUser").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
        });

        modelBuilder.Entity<UserConfiguration>(entity =>
        {
            entity.ToTable("UserConfiguration", "upmconf");

            entity.HasIndex(e => e.RoleId, "IX_UserConfiguration_RoleID");

            entity.HasIndex(e => e.UserId, "IX_UserConfiguration_UserID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Role).WithMany(p => p.UserConfigurations).HasForeignKey(d => d.RoleId);

            entity.HasOne(d => d.User).WithMany(p => p.UserConfigurations).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<ViewActiveColaboradore>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_ActiveColaboradores", "upmdat");

            entity.Property(e => e.CbApeMat)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("CB_APE_MAT");
            entity.Property(e => e.CbApePat)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("CB_APE_PAT");
            entity.Property(e => e.CbCodigo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("CB_CODIGO");
            entity.Property(e => e.CbNombres)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("CB_NOMBRES");
            entity.Property(e => e.Prettyname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("PRETTYNAME");
        });

        modelBuilder.Entity<ViewComponentsTree>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_ComponentsTree", "upmdat");

            entity.Property(e => e.Descripcion).HasMaxLength(80);
            entity.Property(e => e.Estructura).HasMaxLength(30);
            entity.Property(e => e.Familia).HasMaxLength(30);
            entity.Property(e => e.Modelo).HasMaxLength(30);
            entity.Property(e => e.Nivel).HasMaxLength(20);
            entity.Property(e => e.Producto).HasMaxLength(30);
            entity.Property(e => e.Proveedor).HasMaxLength(80);
            entity.Property(e => e.Sitio).HasMaxLength(5);
            entity.Property(e => e.Unidad).HasMaxLength(5);
            entity.Property(e => e.Usage).HasColumnType("decimal(18, 4)");
        });

        modelBuilder.Entity<WorkShift>(entity =>
        {
            entity.ToTable("WorkShift", "upmdat");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
