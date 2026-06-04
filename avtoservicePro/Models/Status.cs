using System;
using System.Collections.Generic;

namespace avtoservicePro.Models;

public partial class Status
{
    public int Id { get; set; }

    public string Status1 { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public override string ToString()
    {
        return Status1;
    }

    public bool StatusIsVisible
    {
        get
        {
            if (Id == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
