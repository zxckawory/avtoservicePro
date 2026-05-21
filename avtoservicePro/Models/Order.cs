using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;

namespace avtoservicePro.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateTime OrderDayTime { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public int CarId { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual ICollection<OrderHistory> OrderHistories { get; set; } = new List<OrderHistory>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public Bitmap ImageBitmap
    {
        get
        {
            try
            {


                if (!string.IsNullOrEmpty(Image))
                {
                    var path = System.IO.Path.Combine(
                        AppContext.BaseDirectory,
                        "Assets",
                        Image
                    );

                    if (System.IO.File.Exists(path))
                    {
                        return new Bitmap(path);
                    }
                }
            }
            catch
            {
                return new Bitmap(AssetLoader.Open(
                new Uri("avares://avtoservicePro/Assets/image_placeholder_resource.png")
            ));
            }

            return new Bitmap(AssetLoader.Open(
                new Uri("avares://avtoservicePro/Assets/image_placeholder_resource.png")
            ));

        }
    }

    public string AllServices
    {
        get
        {
            return string.Join(", ", Services);
        }
    }
}
