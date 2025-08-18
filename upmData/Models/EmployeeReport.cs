using System;
using System.Collections.Generic;

namespace upmData.Models;

public partial class EmployeeReport
{
    public int Id { get; set; }

    public bool Active { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public int CodeUser { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? EndDate { get; set; }

    public Guid LineId { get; set; }

    public Guid WorkShiftId { get; set; }

    public virtual Line Line { get; set; } = null!;

    public virtual WorkShift WorkShift { get; set; } = null!;
}
