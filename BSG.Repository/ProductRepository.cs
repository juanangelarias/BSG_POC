using AutoMapper;
using BSG.Common.DTO;
using BSG.Database;
using BSG.Entities;
using BSG.Repository.Base;

namespace BSG.Repository;

public interface IProductRepository : IRepositoryBase<Product, ProductDto>
{
}

public class ProductRepository(IMapper mapper, BsgDbContext db) 
    : RepositoryBase<Product, ProductDto>(mapper, db), IProductRepository
{
}