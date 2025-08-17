using System;
using System.Collections.Generic;

namespace upmData.Models;

public partial class LiderConfiguration
{
    public int Id { get; set; }

    public bool Active { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public Guid UserId { get; set; }

    public int PartNumberConfigurationId { get; set; }

    public virtual PartNumberConfiguration PartNumberConfiguration { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
