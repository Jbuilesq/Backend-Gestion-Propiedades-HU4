using AutoMapper;
using property.Domain.Entities;

namespace property.Application.DTOs;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<User, AuthResponseDto>();
        CreateMap<AuthResponseDto, User>();
        
        CreateMap<User, RegisterDto>();
        CreateMap<RegisterDto, User>();
        
        CreateMap<User, LoginDto>();
        CreateMap<LoginDto, User>();
    }
}