using System;
using System.Collections.Generic;

namespace Fashion_Avenue.Models;

public partial class Product
{
    public int PId { get; set; }

    public string? PName { get; set; }

    public int? PPrice { get; set; }

    public string? PImage { get; set; }

    public int? PColor { get; set; }

    public int? PCatgId { get; set; }

    public virtual ICollection<LikeCount> LikeCounts { get; } = new List<LikeCount>();

    public virtual ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();

    public virtual Category? PCatg { get; set; }

    public virtual ProductColor? PColorNavigation { get; set; }

    public virtual ICollection<ProductLike> ProductLikes { get; } = new List<ProductLike>();
}
