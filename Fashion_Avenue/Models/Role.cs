using System;
using System.Collections.Generic;

namespace Fashion_Avenue.Models;

public partial class Role
{
    public int RId { get; set; }

    public string? RName { get; set; }

    public virtual ICollection<UserRecord> UserRecords { get; } = new List<UserRecord>();
}
