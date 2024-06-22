using Microsoft.EntityFrameworkCore;

namespace DotK_TechShop.Models.Db;

public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)
{
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Customer> Customers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Brand - Product: 1 - n
        modelBuilder.Entity<Brand>()
            .HasMany(o => o.Products)
            .WithOne(o => o.Brand)
            .HasForeignKey(o => o.BrandID)
            .OnDelete(DeleteBehavior.Cascade);

        // Product - Discount: 1 - n
        modelBuilder.Entity<Discount>()
            .HasOne(o => o.Product)
            .WithMany(o => o.Discounts)
            .HasForeignKey(o => o.ProductID)
            .OnDelete(DeleteBehavior.Cascade);

        // Customer - Order: 1 - n
        modelBuilder.Entity<Customer>()
            .HasMany(o => o.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.Phone)
            .OnDelete(DeleteBehavior.Cascade);

        // Order - OrderDetail: 1 - n
        modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderDetails)
            .WithOne(o => o.Order)
            .HasForeignKey(o => o.OrderID)
            .OnDelete(DeleteBehavior.Cascade);

        // Product - OrderDetail: 1 - n
        modelBuilder.Entity<Product>()
            .HasMany(o => o.OrderDetails)
            .WithOne(o => o.Product)
            .HasForeignKey(o => o.ProductID)
            .OnDelete(DeleteBehavior.Cascade);

    }
}