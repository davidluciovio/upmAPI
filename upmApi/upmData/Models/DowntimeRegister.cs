using System;
using System.Collections.Generic;

namespace upmData.Models;

public partial class DowntimeRegister
{
    public int Id { get; set; }

    public bool Active { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public Guid DowntimeId { get; set; }

    public int PartNumberConfigurationId { get; set; }

    public virtual Downtime Downtime { get; set; } = null!;

    public virtual ICollection<DowntimeRegisterResponsable> DowntimeRegisterResponsables { get; set; } = new List<DowntimeRegisterResponsable>();

    public virtual PartNumberConfiguration PartNumberConfiguration { get; set; } = null!;
}
