using System;
using System.Collections.Generic;

namespace avtoservicePro.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    public virtual Role Role { get; set; } = null!;

    public override string ToString()
    {
        return Name;
    }
}

