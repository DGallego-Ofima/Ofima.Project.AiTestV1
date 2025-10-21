using Microsoft.EntityFrameworkCore;
using Ofima.Orders.Domain.Entities;

namespace Ofima.Orders.Infrastructure.Data;

public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
    {
    }

    // Security Schema
    public DbSet<User> Users { get; set; }

    // ERP Schema
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users", "sec");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).HasMaxLength(80).IsRequired();
            entity.Property(e => e.PasswordHash).HasMaxLength(256).IsRequired();
            entity.Property(e => e.Role).HasMaxLength(40).IsRequired().HasDefaultValue("User");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.IsActive);
            entity.HasIndex(e => e.Role);
        });

        // Configure Customer entity
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers", "erp");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(120).IsRequired();
            entity.Property(e => e.TaxId).HasMaxLength(32).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            
            entity.HasIndex(e => e.TaxId).IsUnique();
            entity.HasIndex(e => e.IsActive);
        });

        // Configure Product entity
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products", "erp");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Sku).HasMaxLength(40).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(120).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            
            entity.HasIndex(e => e.Sku).IsUnique();
            entity.HasIndex(e => e.IsActive);
        });

        // Configure Stock entity
        modelBuilder.Entity<Stock>(entity =>
        {
            entity.ToTable("Stocks", "erp");
            entity.HasKey(e => e.ProductId);
            entity.Property(e => e.OnHand).HasDefaultValue(0);
            entity.Property(e => e.Reserved).HasDefaultValue(0);
            entity.Property(e => e.Available).HasComputedColumnSql("[OnHand] - [Reserved]");
            entity.Property(e => e.LastUpdatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.Property(e => e.RowVersion).IsRowVersion();
            
            entity.HasOne(e => e.Product)
                  .WithOne(p => p.Stock)
                  .HasForeignKey<Stock>(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Order entity
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders", "erp");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Number).HasMaxLength(30).IsRequired();
            entity.Property(e => e.Status).HasConversion<byte>();
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18,2)").HasDefaultValue(0);
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
            entity.Property(e => e.Total).HasColumnType("decimal(18,2)").HasDefaultValue(0);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.RowVersion).IsRowVersion();
            
            entity.HasIndex(e => e.Number).IsUnique();
            entity.HasIndex(e => e.CustomerId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.CreatedAt);
            
            entity.HasOne(e => e.Customer)
                  .WithMany(c => c.Orders)
                  .HasForeignKey(e => e.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);
                  
            entity.HasOne(e => e.CreatedByUser)
                  .WithMany(u => u.Orders)
                  .HasForeignKey(e => e.CreatedBy)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure OrderLine entity
        modelBuilder.Entity<OrderLine>(entity =>
        {
            entity.ToTable("OrderLines", "erp");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Qty).IsRequired();
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.LineTotal).HasComputedColumnSql("[Qty] * [UnitPrice]");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            
            entity.HasIndex(e => e.OrderId);
            entity.HasIndex(e => e.ProductId);
            entity.HasIndex(e => new { e.OrderId, e.ProductId }).IsUnique();
            
            entity.HasOne(e => e.Order)
                  .WithMany(o => o.OrderLines)
                  .HasForeignKey(e => e.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.Product)
                  .WithMany(p => p.OrderLines)
                  .HasForeignKey(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure AuditLog entity
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.ToTable("AuditLog", "erp");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Entity).HasMaxLength(40).IsRequired();
            entity.Property(e => e.Action).HasMaxLength(30).IsRequired();
            entity.Property(e => e.At).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.Property(e => e.IpAddress).HasMaxLength(45);
            entity.Property(e => e.UserAgent).HasMaxLength(500);
            
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Entity);
            entity.HasIndex(e => e.EntityId);
            entity.HasIndex(e => e.At);
            
            entity.HasOne(e => e.User)
                  .WithMany(u => u.AuditLogs)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
