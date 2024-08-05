using System;
using System.Collections.Generic;

namespace Fashion_Avenue.Models;

public partial class UsedCoupon
{
    public int UcId { get; set; }

    public int? UcCouponId { get; set; }

    public int? UcUserId { get; set; }

    public virtual Coupon? UcCoupon { get; set; }

    public virtual UserRecord? UcUser { get; set; }
}
