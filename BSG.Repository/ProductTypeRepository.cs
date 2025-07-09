using AutoMapper;
using BSG.Common.DTO;
using BSG.Common.DTO.Base;
using BSG.Common.Model;
using BSG.Database;
using BSG.Entities;
using BSG.Repository.Base;

namespace BSG.Repository;

public interface IProductTypeRepository : IRepositoryExtended<ProductType, ProductTypeDto>
{
}

public class ProductTypeRepository(IMapper mapper, BsgDbContext db) 
    : RepositoryBase<ProductType, ProductTypeDto>(mapper, db), IProductTypeRepository
{
    public Task<PagedResponse<ProductTypeDto>> GetPageAsync(QueryParams parameters)
    {
        throw new NotImplementedException();
    }
}