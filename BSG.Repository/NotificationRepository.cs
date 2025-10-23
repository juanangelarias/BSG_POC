using AutoMapper;
using BSG.Common.Constants;
using BSG.Common.DTO;
using BSG.Common.Model;
using BSG.Database;
using BSG.Entities;
using BSG.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Repository;

public interface INotificationRepository : IRepositoryExtended<Notification, NotificationDto>
{
    Task<List<NotificationDto>> GetByEmail(string email);
}

public class NotificationRepository(IMapper mapper, BsgDbContext db)
    : RepositoryBase<Notification, NotificationDto>(mapper, db), INotificationRepository
{
    private readonly BsgDbContext _db = db;
    private readonly IMapper _mapper = mapper;

    public async Task<List<NotificationDto>> GetByEmail(string email)
    {
        var qry = await _db.Notifications
            .Include(i=>i.Properties)
            .ThenInclude(t=>t.Property)
            .Include(i => i.Recipients)
            .Where(r => r.SenderEmail == email ||
                        r.Recipients.Any(a => a.Email == email && a.Status != NotificationStatus.Closed))
            .ToListAsync();
        
        return _mapper.Map<List<NotificationDto>>(qry);
    }

    public async Task<PagedResponse<NotificationDto>> GetPageAsync(QueryParams parameters)
    {
        throw new NotImplementedException();
    }
}