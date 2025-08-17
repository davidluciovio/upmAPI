using System;
using System.Collections.Generic;

namespace upmData.Models;

public partial class WorkShift
{
    public Guid Id { get; set; }

    public bool Active { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public string Description { get; set; } = null!;

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int SecondsQuantity { get; set; }

    public virtual ICollection<EmployeeReport> EmployeeReports { get; set; } = new List<EmployeeReport>();
}
