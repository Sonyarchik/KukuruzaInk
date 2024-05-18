using System;
using System.Collections.Generic;

namespace KukuruzaInk.DATABASE;

public partial class Status
{
    public int StatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
