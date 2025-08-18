using System;
using System.Collections.Generic;

namespace upmData.Models;

public partial class DowntimeRegisterResponsable
{
    public int Id { get; set; }

    public bool Active { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public int CodeUser { get; set; }

    public string Name { get; set; } = null!;

    public int DowntimeRegisterId { get; set; }

    public virtual DowntimeRegister DowntimeRegister { get; set; } = null!;
}
