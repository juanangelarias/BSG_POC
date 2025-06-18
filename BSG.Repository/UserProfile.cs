using AutoMapper;
using BSG.Common.DTO;
using BSG.Database;
using BSG.Entities;
using BSG.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace BSG.Repository;

public interface IUserProfileRepository : IRepositoryBase<UserProfile, UserProfileDto>
{
}

public class UserProfileRepository(IMapper mapper, BsgDbContext db) 
    : RepositoryBase<UserProfile, UserProfileDto>(mapper, db), IUserProfileRepository
{
}