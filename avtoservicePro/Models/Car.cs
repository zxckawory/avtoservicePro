using System;
using System.Collections.Generic;

namespace avtoservicePro.Models;

public partial class Car
{
    public int Id { get; set; }

    public string CarName { get; set; } = null!;

    public string CarNumber { get; set; } = null!;

    public int UserId { get; set; }

    public int CarTypeId { get; set; }

    public int HorsePower { get; set; }

    public decimal EngineVolume { get; set; }

    public int FuelTypeId { get; set; }

    public int Year { get; set; }

    public int Mileage { get; set; }

    public virtual CarType CarType { get; set; } = null!;

    public virtual FuelType FuelType { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User User { get; set; } = null!;

    public string CarNameNumber
    {
        get
        {
            return $"{CarName}, {CarNumber}";
        }
    }

    public override string ToString()
    {
        return CarNameNumber;
    }
}
