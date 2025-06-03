using BSG.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Entities;

public class UserPassword: EntityBase, IEntityBase
{
    public long UserId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public byte[]? Key { get; set; }
    public byte[]? Password { get; set; }
    
    //
    
    public User? User { get; set; }
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<UserPassword>( e =>
        {
            MapBaseEntityProperties( e );

            e.Property( p => p.UserId )
                .IsRequired();

            e.Property( p => p.StartDate )
                .IsRequired();

            e.Property( p => p.EndDate )
                .IsRequired();

            e.Property( p => p.Key )
                .IsRequired();

            e.Property( p => p.Password )
                .IsRequired();

            e.HasIndex( i => new { i.UserId, i.StartDate } )
                .IsUnique();
        } );
    }
}