using AutoMapper;
using BSG.Common.DTO;
using BSG.Entities;

namespace BSG.Database.Mappings;

public class SqlMappingsProfile: Profile
{
    public SqlMappingsProfile()
    {
        // U
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<UserPassword, UserPasswordDto>().ReverseMap();
    }
}