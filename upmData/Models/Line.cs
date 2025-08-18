using System;
using System.Collections.Generic;

namespace upmData.Models;

public partial class Line
{
    public Guid Id { get; set; }

    public bool Active { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public string LineName { get; set; } = null!;

    public string WorkCenter { get; set; } = null!;

    public string CodeLine { get; set; } = null!;

    public string? PlcIp { get; set; }

    public virtual ICollection<EmployeeReport> EmployeeReports { get; set; } = new List<EmployeeReport>();

    public virtual ICollection<PartNumberConfiguration> PartNumberConfigurations { get; set; } = new List<PartNumberConfiguration>();
}
