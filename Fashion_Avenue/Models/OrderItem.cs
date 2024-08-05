using System;
using System.Collections.Generic;

namespace Fashion_Avenue.Models;

public partial class OrderItem
{
    public int OrderItemsId { get; set; }

    public int? OrderItemsOrderId { get; set; }

    public int? OrderItemsProdId { get; set; }

    public int? OrderItemsQuantity { get; set; }

    public virtual Order? OrderItemsOrder { get; set; }

    public virtual Product? OrderItemsProd { get; set; }
}
