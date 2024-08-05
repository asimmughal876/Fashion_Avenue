using System;
using System.Collections.Generic;

namespace Fashion_Avenue.Models;

public partial class Coupon
{
    public int CId { get; set; }

    public string? CName { get; set; }

    public decimal? CDiscount { get; set; }

    public virtual ICollection<UsedCoupon> UsedCoupons { get; } = new List<UsedCoupon>();
}
