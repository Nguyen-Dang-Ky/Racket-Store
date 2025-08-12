
using AutoMapper;
using Serberus_Racket_Store.DTOs.UserDTOs;
using Serberus_Racket_Store.Models;
using System; 

namespace Serberus_Racket_Store.MappingProfiles 
{
    public class UserProfiles : Profile
    {
        public UserProfiles()
        {
            CreateMap<Users, UserDto>()
                .ForMember(dest => dest.userId, opt => opt.MapFrom(src => src.userId))
                .ForMember(dest => dest.userCode, opt => opt.MapFrom(src => src.userCode))
                .ForMember(dest => dest.fullName, opt => opt.MapFrom(src => src.fullName))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.email))
                .ForMember(dest => dest.role, opt => opt.MapFrom(src => src.role))
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => src.createAt));

            CreateMap<CreateUserDto, Users>()
                .ForMember(dest => dest.userId, opt => opt.Ignore()) 
                .ForMember(dest => dest.userCode, opt => opt.Ignore()) 
                .ForMember(dest => dest.createAt, opt => opt.MapFrom(src => DateTime.UtcNow)) 
                .ForMember(dest => dest.isDelete, opt => opt.MapFrom(src => false)); 

            CreateMap<UpdateUserDto, Users>()
                .ForMember(dest => dest.userId, opt => opt.Ignore()) 
                .ForMember(dest => dest.userCode, opt => opt.Ignore()) 
                .ForMember(dest => dest.createAt, opt => opt.Ignore()) 
                .ForMember(dest => dest.isDelete, opt => opt.Ignore()) 
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}