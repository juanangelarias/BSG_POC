using AutoMapper;
using BSG.Common.DTO;
using BSG.Database;
using BSG.Entities;
using BSG.Repository.Base;

namespace BSG.Repository;

public interface INotificationPropertyRepository : IRepositoryBase<NotificationProperty, NotificationPropertyDto>
{
}

public class NotificationPropertyRepository(IMapper mapper, BsgDbContext db)
    : RepositoryBase<NotificationProperty, NotificationPropertyDto>(mapper, db),
        INotificationPropertyRepository
{
}