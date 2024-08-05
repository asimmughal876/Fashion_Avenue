using System;
using System.Collections.Generic;

namespace Fashion_Avenue.Models;

public partial class ProductColor
{
    public int PcId { get; set; }

    public string? PcColor { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
