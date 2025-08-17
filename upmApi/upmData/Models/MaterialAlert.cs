using System;
using System.Collections.Generic;

namespace upmData.Models;

public partial class MaterialAlert
{
    public int Id { get; set; }

    public bool Active { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int Status { get; set; }

    public string Component { get; set; } = null!;

    public int PartNumberConfigurationId { get; set; }

    public string? CompleteBy { get; set; }

    public string? ProcessedBy { get; set; }

    public DateTime? CancelDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public DateTime? NotifiedDate { get; set; }

    public DateTime? ReceivedDate { get; set; }

    public virtual PartNumberConfiguration PartNumberConfiguration { get; set; } = null!;
}
