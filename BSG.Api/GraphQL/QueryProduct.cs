using BSG.Database;
using BSG.Entities;
using Microsoft.EntityFrameworkCore;

namespace BSG.Api.GraphQL;

public partial class Query
{
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<List<Product>> GetProducts([Service] BsgDbContext db)
    {
        return await db.Products.Include(i => i.ProductType).ToListAsync();
    }
    
    [UseProjection]
    public async Task<Product?> GetProduct([Service] BsgDbContext db, long id)
    {
        return await db.Products
            .Include(i => i.ProductType)
            .FirstOrDefaultAsync(f => f.Id == id);
    }
}