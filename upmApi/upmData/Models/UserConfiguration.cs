using System;
using System.Collections.Generic;

namespace upmData.Models;

public partial class UserConfiguration
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }

    public bool Active { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
