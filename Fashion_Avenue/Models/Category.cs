using System;
using System.Collections.Generic;

namespace Fashion_Avenue.Models;

public partial class Category
{
    public int CId { get; set; }

    public string? CName { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
