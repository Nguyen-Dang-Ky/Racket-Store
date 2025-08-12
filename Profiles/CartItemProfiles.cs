using AutoMapper;
using Serberus_Racket_Store.Models;
using Serberus_Racket_Store.DTOs.CartItemDTOs;

namespace Serberus_Racket_Store.Profiles
{
    public class CartItemProfiles : Profile
    {
        public CartItemProfiles() 
        {
            CreateMap<CartItems, CartDto>()
                .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.cartItemId))
                .ForMember(dest => dest.CartCode, opt => opt.MapFrom(src => src.cartItemCode))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.userId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Users != null ? src.Users.fullName : null))
                .ForMember(dest => dest.RacketId, opt => opt.MapFrom(src => src.racketId))
                .ForMember(dest => dest.RacketName, opt => opt.MapFrom(src => src.Rackets != null ? src.Rackets.racketName : null))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.quantity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Rackets.price))
                .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.Rackets.imageURL)); 

            CreateMap<CreateCartDto, CartItems>()
                .ForMember(dest => dest.cartItemId, opt => opt.Ignore())
                .ForMember(dest => dest.cartItemCode, opt => opt.Ignore())
                .ForMember(dest => dest.userId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.racketId, opt => opt.MapFrom(src => src.RacketId))
                .ForMember(dest => dest.quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.isDelete, opt => opt.MapFrom(src => false));

            CreateMap<UpdateCartDto, CartItems>()
                .ForMember(dest => dest.cartItemId, opt => opt.Ignore())
                .ForMember(dest => dest.cartItemCode, opt => opt.Ignore())
                .ForMember(dest => dest.userId, opt => opt.Ignore())
                .ForMember(dest => dest.racketId, opt => opt.Ignore())
                .ForMember(dest => dest.quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.isDelete, opt => opt.Ignore());


        }
    }
}
