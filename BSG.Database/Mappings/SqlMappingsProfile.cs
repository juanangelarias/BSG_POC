using AutoMapper;
using BSG.Common.DTO;
using BSG.Common.DTO.Base;
using BSG.Entities;

namespace BSG.Database.Mappings;

public class SqlMappingsProfile: Profile
{
    public SqlMappingsProfile()
    {
        // P
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<ProductType, ProductTypeDto>().ReverseMap();
        // U
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<UserPassword, UserPasswordDto>().ReverseMap();
    }
}