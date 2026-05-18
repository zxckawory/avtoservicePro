using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using avtoservicePro.Models;

namespace avtoservicePro.Context;

public partial class AvtoserviceContext : DbContext
{
    public AvtoserviceContext()
    {
    }

    public AvtoserviceContext(DbContextOptions<AvtoserviceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<CarType> CarTypes { get; set; }

    public virtual DbSet<FuelType> FuelTypes { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost; Database=avtoservice; Username=postgres; Password=123;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_car__pk");

            entity.ToTable("Car");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.CarType).WithMany(p => p.Cars)
                .HasForeignKey(d => d.CarTypeId)
                .HasConstraintName("car_cartype_fk");

            entity.HasOne(d => d.FuelType).WithMany(p => p.Cars)
                .HasForeignKey(d => d.FuelTypeId)
                .HasConstraintName("car__fueltype__fk");

            entity.HasOne(d => d.User).WithMany(p => p.Cars)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("car_user_fk");
        });

        modelBuilder.Entity<CarType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_cartype__pk");

            entity.ToTable("CarType");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<FuelType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_fueltype__pk");

            entity.ToTable("FuelType");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_order__pk");

            entity.ToTable("Order");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.OrderDayTime).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Car).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("order_car_fk");

            entity.HasMany(d => d.Services).WithMany(p => p.Orders)
                .UsingEntity<Dictionary<string, object>>(
                    "OrderService",
                    r => r.HasOne<Service>().WithMany()
                        .HasForeignKey("ServiceId")
                        .HasConstraintName("_orderservice__service_fk"),
                    l => l.HasOne<Order>().WithMany()
                        .HasForeignKey("OrderId")
                        .HasConstraintName("_orderservice__order_fk"),
                    j =>
                    {
                        j.HasKey("OrderId", "ServiceId").HasName("_orderservice__pk");
                        j.ToTable("OrderService");
                    });
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_role__pk");

            entity.ToTable("Role");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_service__pk");

            entity.ToTable("Service");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_client__pk");

            entity.ToTable("User");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("user_role_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
