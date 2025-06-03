using BSG.Common.DTO.Base;
using BSG.Common.Model;
using BSG.Entities.Base;

namespace BSG.Repository.Base;

public interface IRepositoryBase<TEntity, TDto>
    where TEntity : class, IEntityBase
    where TDto : class, IDtoBase
{
    Task<TDto> CreateAsync( TDto dto );
    Task<IEnumerable<TDto>> CreateManyAsync( IEnumerable<TDto> dtos );
    Task<bool> DeleteAsync( long id );
    Task<bool> DeleteManyAsync( IEnumerable<long> ids );
    Task<IEnumerable<TDto>> GetAsync( IQueryable<TEntity>? query = null );
    Task<PagedResponse<TDto>> GetAsync( QueryParams parameters, IQueryable<TEntity>? query = null );
    (IQueryable<T>? query, int rowCount) GetPaginatedQueryable<T>( QueryParams parameters, IQueryable<T>? query = null );
    Task<TDto> GetByIdAsync( long id );
    Task<TDto?> UpdateAsync( TDto dto, TEntity? existingRecord = null );
    Task<IEnumerable<TDto>> UpdateManyAsync( IEnumerable<TDto> dtos, IEnumerable<TEntity>? existingRecords = null );
}