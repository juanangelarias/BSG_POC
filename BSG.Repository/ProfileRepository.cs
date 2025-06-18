using AutoMapper;
using BSG.Common.DTO;
using BSG.Database;
using BSG.Repository.Base;
using Profile = BSG.Entities.Profile;

namespace BSG.Repository;

public interface IProfileRepository: IRepositoryBase<Profile,ProfileDto>
{
}

public class ProfileRepository(IMapper mapper, BsgDbContext db) 
    : RepositoryBase<Profile, ProfileDto>(mapper, db), IProfileRepository
{
}