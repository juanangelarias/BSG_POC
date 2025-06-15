using AutoMapper;
using BSG.Common.DTO;
using BSG.Database;
using BSG.Entities;
using BSG.Repository.Base;

namespace BSG.Repository;

public interface IUserAuthRepository : IRepositoryBase<UserAuth, UserAuthDto>
{
}

public class UserAuthRepository(IMapper mapper, BsgDbContext db) 
    : RepositoryBase<UserAuth, UserAuthDto>(mapper, db), IUserAuthRepository
{
}