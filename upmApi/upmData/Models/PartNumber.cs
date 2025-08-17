using System;
using System.Collections.Generic;

namespace upmData.Models;

public partial class PartNumber
{
    public Guid Id { get; set; }

    public bool Active { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public string PartNumberName { get; set; } = null!;

    public float ObjectiveTime { get; set; }

    public float NetoTime { get; set; }

    public virtual PartNumberConfiguration? PartNumberConfiguration { get; set; }
}
