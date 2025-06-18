using AutoMapper;
using BSG.Common.DTO;
using BSG.Database;
using BSG.Entities;
using Microsoft.EntityFrameworkCore;

namespace BSG.Repository.Base;

public interface IProfileAuthRepository : IRepositoryBase<ProfileAuth, ProfileAuthDto>
{
    Task<List<ProfileAuthDto>> GetByProfile(long profileId);
}

public class ProfileAuthRepository(IMapper mapper, BsgDbContext db) 
    : RepositoryBase<ProfileAuth, ProfileAuthDto>(mapper, db), IProfileAuthRepository
{
    public async Task<List<ProfileAuthDto>> GetByProfile(long profileId)
    {
        var qry = await GetQuery()
            .Include(i=> i.Element)
            .Where(r=> r.ProfileId == profileId)
            .ToListAsync();

        return mapper.Map<List<ProfileAuthDto>>(qry);
    }
}