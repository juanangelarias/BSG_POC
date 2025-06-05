using BSG.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Entities;

public class Product: EntityBase, IEntityBase
{
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public long ProductTypeId { get; set; }
    
    //
    
    public ProductType ProductType { get; set; } = null!;
    
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<Product>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.Code)
                .IsRequired()
                .HasMaxLength(50);

            e.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            e.Property(p => p.Description)
                .HasMaxLength(500);

            e.HasIndex(i => i.Code)
                .IsUnique();

            e.HasIndex(i => i.Name);

            e.HasOne(p => p.ProductType)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.ProductTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}