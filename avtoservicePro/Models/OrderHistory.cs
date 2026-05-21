using System;
using System.Collections.Generic;

namespace avtoservicePro.Models;

public partial class OrderHistory
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int HistoryTypeId { get; set; }

    public DateTime HistoryTime { get; set; }

    public virtual HistoryType HistoryType { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
