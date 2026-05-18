using System;
using System.Collections.Generic;

namespace avtoservicePro.Models;

public partial class CarType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    public override string ToString()
    {
        return Type;
    }
}
