using AutoMapper;
using BSG.Common.DTO;
using BSG.Common.Model;
using BSG.Database;
using BSG.Entities;
using BSG.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Repository;

public interface IProductRepository : IRepositoryExtended<Product, ProductDto>
{
    Task<List<ProductDto>> GetExtended();
    Task<List<ProductDto>> Search(string search);
    Task SeedProducts();
}

public class ProductRepository(IMapper mapper, BsgDbContext db) 
    : RepositoryBase<Product, ProductDto>(mapper, db), IProductRepository
{
    private readonly IMapper _mapper = mapper;
    private readonly BsgDbContext _db = db;

    public Task<PagedResponse<ProductDto>> GetPageAsync(QueryParams parameters)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ProductDto>> GetExtended()
    {
        var list = await _db.Products
            .Include(i => i.ProductType)
            .Take(60000)
            .ToListAsync();

        return _mapper.Map<List<ProductDto>>(list);
    }

    public async Task<List<ProductDto>> Search(string search)
    {
        search = search.ToLower();
        var list = await _db.Products
            .Include(i => i.ProductType)
            .Where(r => r.Name.ToLower().Contains(search) ||
                        r.Description.ToLower().Contains(search) ||
                        r.ProductType.Name.ToLower().Contains(search) ||
                        r.ProductType.Description.ToLower().Contains(search))
            .ToListAsync();
        
        return list
            .Select(p => _mapper.Map<ProductDto>(p))
            .ToList();
    }

    public async Task SeedProducts()
    {
        var productTypes = new List<ProductType>();
        for (var t = 0; t < 10; t++)
        {
            var products = new List<Product>();
            for (var i = 0; i < 10000; i++)
            {
                products.Add(new Product
                {
                    Code = $"{t:000}-{i:00000}",
                    Name = $"Product {t:000}-{i:00000}",
                    Description = $"Product {t:000}-{i:00000} Description"
                });
            }

            productTypes.Add(new ProductType
            {
                Name = $"Product Type {t:000}",
                Description = $"Seeded Product {t:000} Description",
                Products = products
            });
        }
        
        _db.ProductTypes.AddRange(productTypes);
        await _db.SaveChangesAsync();
    }
}