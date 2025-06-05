using AutoMapper;
using BSG.Common.DTO.Base;
using BSG.Database;
using BSG.Entities;
using BSG.Repository.Base;

namespace BSG.Repository;

public interface IProductTypeRepository : IRepositoryBase<ProductType, ProductTypeDto>
{
}

public class ProductTypeRepository(IMapper mapper, BsgDbContext db) 
    : RepositoryBase<ProductType, ProductTypeDto>(mapper, db), IProductTypeRepository
{
}