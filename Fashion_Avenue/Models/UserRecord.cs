using System;
using System.Collections.Generic;

namespace Fashion_Avenue.Models;

public partial class UserRecord
{
    public int UId { get; set; }

    public string? UName { get; set; }

    public string? UEmail { get; set; }

    public string? UPass { get; set; }

    public int? URoleId { get; set; }

    public virtual ICollection<BlogComment> BlogComments { get; } = new List<BlogComment>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual ICollection<ProductLike> ProductLikes { get; } = new List<ProductLike>();

    public virtual Role? URole { get; set; }

    public virtual ICollection<UsedCoupon> UsedCoupons { get; } = new List<UsedCoupon>();
}
