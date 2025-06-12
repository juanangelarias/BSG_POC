using AutoMapper;
using BSG.Common.DTO;
using BSG.Common.Model;
using BSG.Common.Sorts;
using BSG.Database;
using BSG.Entities;
using BSG.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Repository;

public interface IComponentRepository : IRepositoryExtended<Component, ComponentDto>
{
    Task<List<ComponentDto>> GetExtended();
}

public class ComponentRepository(IMapper mapper, BsgDbContext db) 
    : RepositoryBase<Component, ComponentDto>(mapper, db), IComponentRepository
{
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedResponse<ComponentDto>> GetPageAsync(QueryParams parameters)
    {
        var qry = GetQuery();

        var sort = parameters.Sort ?? ComponentSort.Name;

        var filter = string.IsNullOrEmpty( parameters.Filter )
            ? ""
            : parameters.Filter.ToLower();

        if (filter != string.Empty)
            qry = qry
                .Where(r => r.Name!.ToLower().Contains(filter));

        qry = sort switch
        {
            ComponentSort.Name => parameters.Descending
                ? qry.OrderByDescending( o => o.Name )
                : qry.OrderBy( o => o.Name ),
            _ => parameters.Descending
                ? qry.OrderByDescending( o => o.Name )
                : qry.OrderBy( o => o.Name )
        };

        return await GetAsync( parameters, qry );
    }
    
    public async Task<List<ComponentDto>> GetExtended()
    {
        var qry = await GetQuery()
            .Include(i=>i.Elements)
            .ToListAsync();

        return _mapper.Map<List<ComponentDto>>(qry);
    }
}