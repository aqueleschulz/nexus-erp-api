using Microsoft.EntityFrameworkCore;
using NexusERP.API.Domain;

namespace NexusERP.API.Data;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");

            entity.HasKey(e => e.Id)
                  .HasName("PK_Products");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnName("ProductName")
                .HasColumnType("TEXT");

            entity.Property(e => e.Price)
                .IsRequired()
                .HasColumnType("TEXT")
                .HasColumnName("ProductPrice");
            
            entity.Property(e => e.StockQuantity)
                .IsRequired()
                .HasColumnType("INTEGER")
                .HasColumnName("StockQty");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasColumnType("INTEGER")
                .HasColumnName("Available");   

            entity.Property(e => e.Metadata)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v),
                    v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(v) ?? new Dictionary<string, string>()
                )
                .HasColumnType("TEXT")
                .HasColumnName("Metadata");
        }
        );
    }
}