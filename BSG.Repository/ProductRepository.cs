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
            .ToListAsync();

        return _mapper.Map<List<ProductDto>>(list);
    }
}