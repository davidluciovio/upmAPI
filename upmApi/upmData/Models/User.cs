using System;
using System.Collections.Generic;

namespace upmData.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string CodeUser { get; set; } = null!;

    public string? Email { get; set; }

    public string PasswordHash { get; set; } = null!;

    public bool Active { get; set; }

    public DateTime CreateDate { get; set; }

    public string CreateBy { get; set; } = null!;

    public virtual ICollection<LiderConfiguration> LiderConfigurations { get; set; } = new List<LiderConfiguration>();

    public virtual ICollection<UserConfiguration> UserConfigurations { get; set; } = new List<UserConfiguration>();
}
