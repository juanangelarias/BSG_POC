using AutoMapper;
using BSG.Common.DTO;
using BSG.Database;
using BSG.Entities;
using BSG.Repository.Base;

namespace BSG.Repository;

public interface IComponentRepository : IRepositoryBase<Component, ComponentDto>
{
}

public class ComponentRepository(IMapper mapper, BsgDbContext db) 
    : RepositoryBase<Component, ComponentDto>(mapper, db), IComponentRepository
{
}