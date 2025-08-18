using System;
using System.Collections.Generic;

namespace upmData.Models;

public partial class PartNumberConfiguration
{
    public int Id { get; set; }

    public bool Active { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public Guid PartNumberId { get; set; }

    public Guid LineId { get; set; }

    public int ModelId { get; set; }

    public Guid SupplyAreaId { get; set; }

    public string? PartNumberCode { get; set; }

    public virtual ICollection<DowntimeRegister> DowntimeRegisters { get; set; } = new List<DowntimeRegister>();

    public virtual ICollection<LiderConfiguration> LiderConfigurations { get; set; } = new List<LiderConfiguration>();

    public virtual Line Line { get; set; } = null!;

    public virtual ICollection<MaterialAlert> MaterialAlerts { get; set; } = new List<MaterialAlert>();

    public virtual Model Model { get; set; } = null!;

    public virtual PartNumber PartNumber { get; set; } = null!;

    public virtual ICollection<ProductionRegister> ProductionRegisters { get; set; } = new List<ProductionRegister>();

    public virtual ICollection<RackRegister> RackRegisters { get; set; } = new List<RackRegister>();

    public virtual ICollection<RequerimentsHistory> RequerimentsHistories { get; set; } = new List<RequerimentsHistory>();

    public virtual SupplyArea SupplyArea { get; set; } = null!;
}
