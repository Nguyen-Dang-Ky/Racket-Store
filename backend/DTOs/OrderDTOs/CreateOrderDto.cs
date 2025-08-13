using System.ComponentModel.DataAnnotations;
using Serberus_Racket_Store.DTOs.OrderItemDTOs; 
using Serberus_Racket_Store.DTOs.ShippingInfoDTOs; 

namespace Serberus_Racket_Store.DTOs.OrderDTOs
{
    public class CreateOrderDto
    {
        [Required(ErrorMessage = "Mã người dùng là bắt buộc.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã người dùng phải là một số dương.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Thông tin vận chuyển là bắt buộc.")]
        public CreateShippingInfoDto Shipping_Info { get; set; } = new CreateShippingInfoDto(); 

        [Required(ErrorMessage = "Đơn hàng phải có ít nhất một mặt hàng.")]
        [MinLength(1, ErrorMessage = "Đơn hàng phải có ít nhất một mặt hàng.")]
        public ICollection<CreateOrderItemDto> OrderItems { get; set; } = new List<CreateOrderItemDto>(); 
    }
}