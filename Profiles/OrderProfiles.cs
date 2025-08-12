using AutoMapper;
using Serberus_Racket_Store.DTOs.OrderDTOs;
using Serberus_Racket_Store.Models;
using Serberus_Racket_Store.DTOs.ShippingInfoDTOs;
using Serberus_Racket_Store.DTOs.OrderItemDTOs;

namespace Serberus_Racket_Store.Profiles
{
    public class OrderProfiles : Profile
    {
        public OrderProfiles() 
        {
            CreateMap<Orders, OrderDto>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.orderId))
                .ForMember(dest => dest.OrderCode, opt => opt.MapFrom(src => src.orderCode))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.userId))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.orderDate))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.total))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.status))
                .ForMember(dest => dest.Shipping_Info, opt => opt.MapFrom(src => src.Shipping_Info))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<CreateOrderDto, Orders>()
                .ForMember(dest => dest.orderId, opt => opt.Ignore())
                .ForMember(dest => dest.orderCode, opt => opt.Ignore())
                .ForMember(dest => dest.userId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.orderDate, opt => opt.Ignore())
                .ForMember(dest => dest.total, opt => opt.Ignore())
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => "Pending"))
                .ForMember(dest => dest.isDelete, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Shipping_Info, opt => opt.MapFrom(src => src.Shipping_Info))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<UpdateOrderDto, Orders>()
                .ForMember(dest => dest.orderId, opt => opt.Ignore())
                .ForMember(dest => dest.orderCode, opt => opt.Ignore())
                .ForMember(dest => dest.userId, opt => opt.Ignore())
                .ForMember(dest => dest.orderDate, opt => opt.Ignore())
                .ForMember(dest => dest.total, opt => opt.Ignore())
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.isDelete, opt => opt.Ignore())
                .ForMember(dest => dest.Shipping_Info, opt => opt.Ignore())
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore());
        }
    }
}
