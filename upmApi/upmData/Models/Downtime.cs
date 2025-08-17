using System;
using System.Collections.Generic;

namespace upmData.Models;

public partial class Downtime
{
    public Guid Id { get; set; }

    public bool Active { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string InforCode { get; set; } = null!;

    public string Plccode { get; set; } = null!;

    public bool IsDirectDowntime { get; set; }

    public bool Programable { get; set; }

    public virtual ICollection<DowntimeRegister> DowntimeRegisters { get; set; } = new List<DowntimeRegister>();
}
