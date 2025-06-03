using AutoMapper;
using BSG.Common.DTO.Base;
using BSG.Common.Model;
using BSG.Database;
using BSG.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Repository.Base;

public class RepositoryBase<TEntity, TDto>(IMapper mapper, BsgDbContext db): IRepositoryBase<TEntity, TDto>
    where TEntity : class, IEntityBase
    where TDto : class, IDtoBase
{

    public virtual async Task<TDto> CreateAsync(TDto dto)
    {
        await using var t = await db.Database.BeginTransactionAsync();

        try
        {
            var newRecord = mapper.Map<TEntity>(dto);
            await db.Set<TEntity>().AddAsync(newRecord);
            await db.SaveChangesAsync();
            await t.CommitAsync();

            return mapper.Map<TDto>(newRecord);
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public virtual async Task<IEnumerable<TDto>> CreateManyAsync(IEnumerable<TDto> dtos)
    {
        await using var t = await db.Database.BeginTransactionAsync();

        try
        {
            var newRecords = new List<TEntity>();

            foreach (var dto in dtos)
            {
                var entity = mapper.Map<TEntity>(dto);
                newRecords.Add(entity);
            }

            await db.Set<TEntity>().AddRangeAsync(newRecords);
            await db.SaveChangesAsync();
            await t.CommitAsync();

            return mapper.Map<IEnumerable<TDto>>(newRecords);
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public virtual async Task<bool> DeleteAsync(long id)
    {
        await using var t = await db.Database.BeginTransactionAsync();

        try
        {
            var existingRecord = await GetQuery().FirstOrDefaultAsync(x => x.Id == id);

            if (existingRecord == null)
            {
                throw new KeyNotFoundException();
            }

            db.Set<TEntity>().Remove(existingRecord);
            
            await db.SaveChangesAsync();
            await t.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public virtual async Task<bool> DeleteManyAsync(IEnumerable<long> ids)
    {
        await using var t = await db.Database.BeginTransactionAsync();

        try
        {
            var existingRecords = await GetQuery()
                .Where(record => ids.Contains(record.Id)).ToListAsync();
            
            db.Set<TEntity>().RemoveRange(existingRecords);

            await db.SaveChangesAsync();
            await t.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public virtual async Task<IEnumerable<TDto>> GetAsync(IQueryable<TEntity>? query = null)
    {
        var qry = query ?? GetQuery();

        var queryResult = await qry
            .ToListAsync();
        return mapper.Map<IEnumerable<TDto>>(queryResult);
    }

    public virtual async Task<PagedResponse<TDto>> GetAsync(QueryParams parameters, IQueryable<TEntity>? query = null)
    {
        var q = query ?? GetQuery();
        var queryable = GetPaginatedQueryable(parameters, q);

        var result = await queryable.query!
            .ToListAsync();
        var mappedResult = mapper.Map<IEnumerable<TDto>>(result);
        return new PagedResponse<TDto>(mappedResult, queryable.rowCount);
    }

    public (IQueryable<T>? query, int rowCount) GetPaginatedQueryable<T>(QueryParams parameters, IQueryable<T>? qry)
    {
        if (parameters.PageSize < 1)
        {
            throw new ArgumentException("Page Size cannot be 0");
        }
        
        if (parameters.PageIndex < 0)
        {
            return (null, 0);
        }
        
        var rowCount = (qry?
            .Count()) ?? 0;

        var skip = rowCount <= parameters.PageSize ? 0 : parameters.PageSize * parameters.PageIndex;

        var pagedQuery = qry?
            .Skip(skip)
            .Take(parameters.PageSize);

        return (pagedQuery, rowCount);
    }
    
    public virtual async Task<TDto> GetByIdAsync(long id)
    {
        var record = await GetQuery()
            .FirstOrDefaultAsync(x => x.Id == id);

        return mapper.Map<TDto>(record);
    }

    public virtual async Task<TDto?> UpdateAsync(TDto dto, TEntity? existingRecord = null)
    {
        await using var t = await db.Database.BeginTransactionAsync();

        try
        {
            existingRecord ??= await GetQuery()
                .FirstOrDefaultAsync(record => record.Id == dto.Id);

            if (existingRecord == null)
            {
                return null;
            }

            mapper.Map(dto, existingRecord);
            db.Update(existingRecord);
            await db.SaveChangesAsync();
            await t.CommitAsync();

            return mapper.Map<TDto>(existingRecord);
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public virtual async Task<TDto> UpdateAsync(TDto dto, IQueryable<TEntity> trackingQuery)
    {
        await using var t = await db.Database.BeginTransactionAsync();

        try
        {
            var existingRecord = await trackingQuery.FirstOrDefaultAsync();
            mapper.Map(dto, existingRecord);
            await db.SaveChangesAsync();
            await t.CommitAsync();

            var updatedRecord = await db.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == dto.Id);

            return mapper.Map<TDto>(updatedRecord);
        }
        catch
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public virtual async Task<IEnumerable<TDto>> UpdateManyAsync(IEnumerable<TDto> dtos, IEnumerable<TEntity>? existingRecords = null)
    {
        await using var t = await db.Database.BeginTransactionAsync();

        try
        {
            existingRecords ??= await GetQuery()
                .Where(record => dtos.Select(dto => dto.Id).Contains(record.Id))
                .ToListAsync();

            var baseEntities = existingRecords
                .ToList();
            mapper.Map(dtos, baseEntities);

            db.UpdateRange(baseEntities);
            await db.SaveChangesAsync();
            await t.CommitAsync();

            return mapper.Map<IEnumerable<TDto>>(existingRecords);
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    protected virtual IQueryable<TEntity> GetQuery()
    {
        return db.Set<TEntity>()
                .AsNoTracking();
    }

    /*private static string GetProperPropertyName(DbContext context, string s)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        var property = context.Model.FindEntityType(typeof(TEntity))
            .GetDerivedTypesInclusive()
            .SelectMany(x => x.GetProperties()).Distinct()
            .FirstOrDefault(x => string.Equals(x.Name, s, StringComparison.CurrentCultureIgnoreCase));

        return property != null ? property.Name : string.Empty;
    }*/

    public virtual async Task<bool> DeletePermanentAsync(long id)
    {
        await using var t = await db.Database.BeginTransactionAsync();

        try
        {
            var existingRecord = await GetQuery().FirstOrDefaultAsync(x => x.Id == id);

            if (existingRecord == null)
            {
                return false;
            }
            db.Set<TEntity>().Remove(existingRecord);

            await db.SaveChangesAsync();
            await t.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public virtual async Task<bool> DeletePermanentManyAsync(IEnumerable<long> ids)
    {
        await using var t = await db.Database.BeginTransactionAsync();

        try
        {
            var existingRecords = await GetQuery()
                .Where(record => ids.Contains(record.Id)).ToListAsync();

            db.Set<TEntity>().RemoveRange(existingRecords);

            await db.SaveChangesAsync();
            await t.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    //Detach all entities
    public void DetachAllEntities()
    {
        var changedEntriesCopy = db.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || 
                      e.State == EntityState.Deleted || 
                      e.State == EntityState.Modified ||
                      e.State == EntityState.Unchanged)
            .ToList();

        
        foreach (var entry in changedEntriesCopy)
            entry.State = EntityState.Detached;
    }
}