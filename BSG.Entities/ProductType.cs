using BSG.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Entities;

public class ProductType: EntityBase, IEntityBase
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    
    //

    public List<Product> Products { get; set; } = [];
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<ProductType>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            e.Property(p => p.Description)
                .HasMaxLength(500);

            e.HasIndex(i => i.Name)
                .IsUnique();
        });
    }
}