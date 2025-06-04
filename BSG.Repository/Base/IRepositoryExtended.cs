using BSG.Common.Model;
using BSG.Entities.Base;

namespace BSG.Repository.Base;

public interface IRepositoryExtended<TEntity, TDto> : IRepositoryBase<TEntity, TDto>
    where TEntity : class, IEntityBase
    where TDto : class
{
    Task<PagedResponse<TDto>> GetPageAsync(QueryParams parameters);
}