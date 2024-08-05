using System;
using System.Collections.Generic;

namespace Fashion_Avenue.Models;

public partial class BlogComment
{
    public int BlogCId { get; set; }

    public string? BlogCName { get; set; }

    public int? BlogId { get; set; }

    public int? BlogUId { get; set; }

    public virtual UserRecord? Blo { get; set; }

    public virtual Blog? Blog { get; set; }
}
