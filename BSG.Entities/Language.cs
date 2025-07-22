using BSG.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Entities;

public class Language: EntityBase, IEntityBase
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<Language>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.Code)
                .IsRequired()
                .HasMaxLength(4);

            e.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            e.HasIndex(i => i.Code)
                .IsUnique();
        });
    }
}