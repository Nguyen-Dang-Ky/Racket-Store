using AutoMapper;
using Serberus_Racket_Store.DTOs.ShippingInfoDTOs;
using Serberus_Racket_Store.DTOs.UserDTOs;
using Serberus_Racket_Store.Models;

namespace Serberus_Racket_Store.Profiles
{
    public class ShippingInfoProfiles : Profile
    {
        public ShippingInfoProfiles() 
        {
            CreateMap<Shipping_Info, ShippingInfoDto>()
                .ForMember(dest => dest.ShippingId, opt => opt.MapFrom(src => src.shippingId))
                .ForMember(dest => dest.ShippingCode, opt => opt.MapFrom(src => src.shippingCode))
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.orderId))
                .ForMember(dest => dest.ReceiverName, opt => opt.MapFrom(src => src.receiverName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.phoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.address))
                .ForMember(dest => dest.ShippingMethod, opt => opt.MapFrom(src => src.shippingMethod))
                .ForMember(dest => dest.ShippingFee, opt => opt.MapFrom(src => src.shippingFee));

            CreateMap<CreateShippingInfoDto, Shipping_Info>()
                .ForMember(dest => dest.shippingId, opt => opt.Ignore())
                .ForMember(dest => dest.shippingCode, opt => opt.Ignore())
                .ForMember(dest => dest.orderId, opt => opt.Ignore())
                .ForMember(dest => dest.receiverName, opt => opt.MapFrom(src => src.ReceiverName))
                .ForMember(dest => dest.phoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.shippingMethod, opt => opt.MapFrom(src => src.ShippingMethod))
                .ForMember(dest => dest.shippingFee, opt => opt.Ignore())
                .ForMember(dest => dest.isDelete, opt => opt.MapFrom(src => false));

            CreateMap<UpdateShippingInfoDto, Shipping_Info>()
                .ForMember(dest => dest.shippingId, opt => opt.Ignore())
                .ForMember(dest => dest.shippingCode, opt => opt.Ignore())
                .ForMember(dest => dest.orderId, opt => opt.Ignore())
                .ForMember(dest => dest.receiverName, opt => opt.MapFrom(src => src.ReceiverName))
                .ForMember(dest => dest.phoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.shippingMethod, opt => opt.MapFrom(src => src.ShippingMethod))
                .ForMember(dest => dest.shippingFee, opt => opt.Ignore())
                .ForMember(dest => dest.isDelete, opt => opt.MapFrom(src => false));
        }
    }
}
