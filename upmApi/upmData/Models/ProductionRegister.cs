using System;
using System.Collections.Generic;

namespace upmData.Models;

public partial class ProductionRegister
{
    public int Id { get; set; }

    public bool Active { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public int Quantity { get; set; }

    public int Counter { get; set; }

    public int Order { get; set; }

    public int PartNumberConfigurationId { get; set; }

    public virtual PartNumberConfiguration PartNumberConfiguration { get; set; } = null!;
}
