using AutoMapper;
using BSG.Common;
using BSG.Common.DTO;
using BSG.Database;
using BSG.Entities;
using BSG.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Repository;

public interface IElementRepository: IRepositoryBase<Element, ElementDto>
{
    Task<List<Element>> GetByComponentIdAsync(long componentId);
}

public class ElementRepository(IMapper mapper, BsgDbContext db)
: RepositoryBase<Element, ElementDto>(mapper, db), IElementRepository
{
    private readonly IMapper _mapper = mapper;
    private readonly BsgDbContext _db = db;

    public async Task<List<Element>> GetByComponentIdAsync(long componentId)
    {
        var query = await _db.Elements
            .Where(r => r.ComponentId == componentId)
            .OrderBy(o => o.ComponentId)
            .ThenBy(t => t.Code)
            .ToListAsync();
        
        return _mapper.Map<List<Element>>(query);
    }
}