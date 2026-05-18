using System;
using System.Collections.Generic;

namespace avtoservicePro.Models;

public partial class Service
{
    public int Id { get; set; }

    public string ServiceName { get; set; } = null!;

    public int ServiceCost { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public override string ToString()
    {
        return ServiceName;
    }
}
