using System;
using System.Collections.Generic;

namespace Fashion_Avenue.Models;

public partial class ProductLike
{
    public int PlId { get; set; }

    public int? PlCount { get; set; }

    public int? PlUser { get; set; }

    public int? PlProdId { get; set; }

    public virtual Product? PlProd { get; set; }

    public virtual UserRecord? PlUserNavigation { get; set; }
}
