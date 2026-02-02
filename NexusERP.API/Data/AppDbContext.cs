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
                .HasColumnType("TEXT")
                .HasColumnName("Metadata")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v),
                    v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(v) ?? new Dictionary<string, string>()
                )
                .Metadata.SetValueComparer(new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<Dictionary<string, string>>(
                    (c1, c2) => System.Text.Json.JsonSerializer.Serialize(c1, (System.Text.Json.JsonSerializerOptions?)null) == System.Text.Json.JsonSerializer.Serialize(c2, (System.Text.Json.JsonSerializerOptions?)null),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(System.Text.Json.JsonSerializer.Serialize(c, (System.Text.Json.JsonSerializerOptions?)null), (System.Text.Json.JsonSerializerOptions?)null)!
                ));
        }
        );
    }
}