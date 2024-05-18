using System;
using System.Collections.Generic;

namespace KukuruzaInk.DATABASE;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public DateTime OrderDate { get; set; }

    public int StatusId { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Status Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
