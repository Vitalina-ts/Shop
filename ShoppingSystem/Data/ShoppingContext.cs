using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Models;
namespace ShoppingSystem.Data;
public class ShoppingContext(DbContextOptions<ShoppingContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Supermarket> Supermarkets { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetails> OrderDetails { get; set; }
    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<Product>().Property(p => p.Price).HasPrecision(18, 2);
        model.Entity<Product>().ToTable(t => t.HasCheckConstraint("CK_Products_Price", "[Price] >= 0"));
        model.Entity<Customer>().ToTable(t => t.HasCheckConstraint("CK_Customers_Discount", "[Discount] BETWEEN 0 AND 100"));
        model.Entity<OrderDetails>().ToTable(t => t.HasCheckConstraint("CK_OrderDetails_Quantity", "[Quantity] > 0"));
        model.Entity<OrderDetails>().HasIndex(d => new { d.OrderId, d.ProductId }).IsUnique();
        model.Entity<Order>().HasOne(o => o.Customer).WithMany(c => c.Orders).HasForeignKey(o => o.CustomerId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Order>().HasOne(o => o.Supermarket).WithMany(s => s.Orders).HasForeignKey(o => o.SupermarketId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<OrderDetails>().HasOne(d => d.Order).WithMany(o => o.OrderDetails).HasForeignKey(d => d.OrderId).OnDelete(DeleteBehavior.Cascade);
        model.Entity<OrderDetails>().HasOne(d => d.Product).WithMany(p => p.OrderDetails).HasForeignKey(d => d.ProductId).OnDelete(DeleteBehavior.Restrict);
    }
}