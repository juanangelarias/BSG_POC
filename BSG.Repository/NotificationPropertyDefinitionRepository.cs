using AutoMapper;
using BSG.Common.DTO;
using BSG.Common.Model;
using BSG.Database;
using BSG.Entities;
using BSG.Repository.Base;

namespace BSG.Repository;

public interface
    INotificationPropertyDefinitionRepository
    : IRepositoryExtended<NotificationPropertyDefinition, NotificationPropertyDefinitionDto>
{
}

public class NotificationPropertyDefinitionRepository(IMapper mapper, BsgDbContext db)
    : RepositoryBase<NotificationPropertyDefinition, NotificationPropertyDefinitionDto>(mapper, db),
        INotificationPropertyDefinitionRepository
{
    public async Task<PagedResponse<NotificationPropertyDefinitionDto>> GetPageAsync(QueryParams parameters)
    {
        throw new NotImplementedException();
    }
}