using AutoMapper;
using BSG.Common.DTO;
using BSG.Common.Model;
using BSG.Common.Sorts;
using BSG.Database;
using BSG.Entities;
using BSG.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Repository;

public interface IElementRepository: IRepositoryExtended<Element, ElementDto>
{
    Task<List<ElementDto>> GetByComponentIdAsync(long componentId);
}

public class ElementRepository(IMapper mapper, BsgDbContext db)
    : RepositoryBase<Element, ElementDto>(mapper, db), IElementRepository
{
    private readonly IMapper _mapper = mapper;
    private readonly BsgDbContext _db = db;

    public async Task<List<ElementDto>> GetByComponentIdAsync(long componentId)
    {
        var query = await _db.Elements
            .Where(r => r.ComponentId == componentId)
            .OrderBy(o => o.ComponentId)
            .ThenBy(t => t.Code)
            .ToListAsync();
        
        return _mapper.Map<List<ElementDto>>(query);
    }
    
    public async Task<PagedResponse<ElementDto>> GetPageAsync(QueryParams parameters)
    {
        var qry = GetQuery();

        var sort = parameters.Sort ?? ElementSort.Name;

        var filter = string.IsNullOrEmpty( parameters.Filter )
            ? ""
            : parameters.Filter.ToLower();

        qry = qry.Include(i=>i.Component);
        
        if (filter != string.Empty)
            qry = qry
                .Where(r => r.Name.ToLower().Contains(filter));
        
        qry = sort switch
        {
            ElementSort.Name => parameters.Descending
                ? qry.OrderByDescending( o => o.Name )
                : qry.OrderBy( o => o.Name ),
            ElementSort.Component => parameters.Descending
                ? qry.OrderByDescending( o => o.Component.Name )
                : qry.OrderBy( o => o.Component.Name ),
            _ => parameters.Descending
                ? qry.OrderByDescending( o => o.Name )
                : qry.OrderBy( o => o.Name ),
        };

        return await GetAsync( parameters, qry );
    }
}