using System;
using System.Collections.Generic;

namespace upmData.Models;

public partial class Model
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool Active { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public virtual ICollection<PartNumberConfiguration> PartNumberConfigurations { get; set; } = new List<PartNumberConfiguration>();
}
