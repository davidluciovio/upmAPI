using System;
using System.Collections.Generic;

namespace upmData.Models;

public partial class RackRegister
{
    public int Id { get; set; }

    public bool Active { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public string NoRack { get; set; } = null!;

    public string Serial { get; set; } = null!;

    public string Type { get; set; } = null!;

    public int PartNumberConfigurationId { get; set; }

    public virtual PartNumberConfiguration PartNumberConfiguration { get; set; } = null!;
}
