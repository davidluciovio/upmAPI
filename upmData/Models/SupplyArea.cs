using System;
using System.Collections.Generic;

namespace upmData.Models;

public partial class SupplyArea
{
    public Guid Id { get; set; }

    public bool Active { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<PartNumberConfiguration> PartNumberConfigurations { get; set; } = new List<PartNumberConfiguration>();
}
