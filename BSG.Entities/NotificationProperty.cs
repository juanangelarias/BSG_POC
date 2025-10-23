using BSG.Common.Constants;
using BSG.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Entities;

public class NotificationProperty: EntityBase, IEntityBase
{
    public long NotificationId { get; set; }
    public long NotificationPropertyDefinitionId { get; set; }
    public string TextValue { get; set; } = null!;
    
    
    //public virtual Notification Notification { get; set; } = null!;
    public virtual NotificationPropertyDefinition Property { get; set; } = null!;
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<NotificationProperty>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.NotificationId)
                .IsRequired();

            e.Property(p => p.NotificationPropertyDefinitionId)
                .IsRequired();

            e.Property(p => p.TextValue)
                .HasMaxLength(150)
                .IsRequired();
            
            e.HasOne(o=>o.Property)
                .WithMany()
                .HasForeignKey(k=>k.NotificationPropertyDefinitionId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}