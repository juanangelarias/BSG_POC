using AutoMapper;
using BSG.Common;
using BSG.Common.DTO;
using BSG.Common.DTO.Base;
using BSG.Entities;
using Profile = AutoMapper.Profile;

namespace BSG.Database.Mappings;

public class SqlMappingsProfile: Profile
{
    public SqlMappingsProfile()
    {
        // C
        CreateMap<Component, ComponentDto>().ReverseMap();
        // E
        CreateMap<Element, ElementDto>().ReverseMap();
        // P
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<ProductType, ProductTypeDto>().ReverseMap();
        CreateMap<Profile, ProfileDto>().ReverseMap();
        CreateMap<ProfileAuth, ProfileAuthDto>().ReverseMap();
        // U
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<UserAuth, UserAuthDto>().ReverseMap();
        CreateMap<User, UserExtendedDto>().ReverseMap();
        CreateMap<UserPassword, UserPasswordDto>().ReverseMap();
        CreateMap<UserProfile, UserPasswordDto>().ReverseMap();
    }
}