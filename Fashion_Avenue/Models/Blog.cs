using System;
using System.Collections.Generic;

namespace Fashion_Avenue.Models;

public partial class Blog
{
    public int BId { get; set; }

    public string? BTitle { get; set; }

    public string? BDesc { get; set; }

    public string? BImage { get; set; }

    public DateTime? BDate { get; set; }

    public virtual ICollection<BlogComment> BlogComments { get; } = new List<BlogComment>();
}
