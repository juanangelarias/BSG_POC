using BSG.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Entities;

public class NotificationRecipient: EntityBase, IEntityBase
{
    public long NotificationId { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string? PhoneNumber { get; set; } = "";
    public DateTime? Snoozed { get; set; }
    public string Status { get; set; } = "";


    public Notification Notification { get; set; } = null!;
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<NotificationRecipient>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.NotificationId)
                .IsRequired();

            e.Property(p => p.Name)
                .HasMaxLength(150)
                .IsRequired();

            e.Property(p => p.Email)
                .HasMaxLength(250)
                .IsRequired();

            e.Property(p => p.PhoneNumber)
                .HasMaxLength(20);

            e.HasIndex(i => new { i.NotificationId, i.Email, i.Snoozed });
        });
    }
}