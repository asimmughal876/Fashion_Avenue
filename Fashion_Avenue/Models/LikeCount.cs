using System;
using System.Collections.Generic;

namespace Fashion_Avenue.Models;

public partial class LikeCount
{
    public int LcId { get; set; }

    public int? LcCount { get; set; }

    public int? LcProdId { get; set; }

    public virtual Product? LcProd { get; set; }
}
