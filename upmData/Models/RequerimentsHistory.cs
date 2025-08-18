using System;
using System.Collections.Generic;

namespace upmData.Models;

public partial class RequerimentsHistory
{
    public int Id { get; set; }

    public bool Active { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public string Plant01 { get; set; } = null!;

    public string Plant02 { get; set; } = null!;

    public DateTime ProductionDate { get; set; }

    public float RanQuantity { get; set; }

    public int Production { get; set; }

    public int PartNumberConfigurationId { get; set; }

    public virtual PartNumberConfiguration PartNumberConfiguration { get; set; } = null!;
}
