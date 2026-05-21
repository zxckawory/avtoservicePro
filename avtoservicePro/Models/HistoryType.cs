using System;
using System.Collections.Generic;

namespace avtoservicePro.Models;

public partial class HistoryType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<OrderHistory> OrderHistories { get; set; } = new List<OrderHistory>();

    public override string ToString()
    {
        return Type;
    }

    public bool TypeIsVisible
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

    public string ImageType
    {
        get
        {
            if(Id == 1)
            {
                return "CheckCircleOutline";
            }
            else
            {
                return "PencilOutline";
            }
        }
    }
}
