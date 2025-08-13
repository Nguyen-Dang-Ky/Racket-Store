using AutoMapper;
using Serberus_Racket_Store.Models;
using Serberus_Racket_Store.DTOs.RacketDTOs;

namespace Serberus_Racket_Store.MappingProfiles
{
    public class RacketProfile : Profile
    {
        public RacketProfile()
        {
            CreateMap<Rackets, RacketDto>()
                .ForMember(dest => dest.RacketId, opt => opt.MapFrom(src => src.racketId))
                .ForMember(dest => dest.RacketCode, opt => opt.MapFrom(src => src.racketCode))
                .ForMember(dest => dest.RacketName, opt => opt.MapFrom(src => src.racketName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.price))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.quantity))
                .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.imageURL))
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.brandId))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brands != null ? src.Brands.brandName : null));

            CreateMap<CreateRacketDto, Rackets>()
                .ForMember(dest => dest.racketId, opt => opt.Ignore()) 
                .ForMember(dest => dest.racketCode, opt => opt.Ignore()) 
                .ForMember(dest => dest.isDelete, opt => opt.MapFrom(src => false)); 

            CreateMap<UpdateRacketDto, Rackets>()
                .ForMember(dest => dest.racketId, opt => opt.Ignore())
                .ForMember(dest => dest.racketCode, opt => opt.Ignore())
                .ForMember(dest => dest.isDelete, opt => opt.Ignore());
        }
    }
}