using Microsoft.EntityFrameworkCore;

namespace BSG.Entities.Base;

public interface IConfigurableEntity
{
    void OnModelCreating( ModelBuilder m );
}