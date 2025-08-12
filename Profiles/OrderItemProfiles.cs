using AutoMapper;
using Serberus_Racket_Store.DTOs.OrderItemDTOs;
using Serberus_Racket_Store.Models;

namespace Serberus_Racket_Store.Profiles
{
    public class OrderItemProfiles : Profile
    {
        public OrderItemProfiles() 
        {
            CreateMap<OrderItems, OrderItemDto>()
                .ForMember(dest => dest.OrderItemId, opt => opt.MapFrom(src => src.orderItemId))
                .ForMember(dest => dest.OrderItemCode, opt => opt.MapFrom(src => src.orderItemCode))
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.orderId))
                .ForMember(dest => dest.RacketId, opt => opt.MapFrom(src => src.racketId))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.price));

            CreateMap<CreateOrderItemDto, OrderItems>()
                .ForMember(dest => dest.orderItemId, opt => opt.Ignore())
                .ForMember(dest => dest.orderItemCode, opt => opt.Ignore())
                .ForMember(dest => dest.orderId, opt => opt.Ignore())
                .ForMember(dest => dest.racketId, opt => opt.Ignore())
                .ForMember(dest => dest.price, opt => opt.Ignore())
                .ForMember(dest => dest.isDelete, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.racketId, opt => opt.MapFrom(src => src.RacketId))
                .ForMember(dest => dest.quantity, opt => opt.MapFrom(src => src.Quantity));


            CreateMap<UpdateOrderItemDto, OrderItems>()
                .ForMember(dest => dest.orderItemId, opt => opt.Ignore())
                .ForMember(dest => dest.orderItemCode, opt => opt.Ignore())
                .ForMember(dest => dest.orderId, opt => opt.Ignore())
                .ForMember(dest => dest.racketId, opt => opt.Ignore())
                .ForMember(dest => dest.price, opt => opt.Ignore())
                .ForMember(dest => dest.isDelete, opt => opt.Ignore())
                .ForMember(dest => dest.quantity, opt => opt.MapFrom(src => src.Quantity));

;        }
    }
}
