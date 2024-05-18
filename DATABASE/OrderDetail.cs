using System;
using System.Collections.Generic;

namespace KukuruzaInk.DATABASE;

public partial class OrderDetail
{
    public int DetailId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int? ProductId2 { get; set; }

    public int? ProductId3 { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
