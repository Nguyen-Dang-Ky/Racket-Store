using System.ComponentModel.DataAnnotations;

namespace Serberus_Racket_Store.DTOs.OrderItemDTOs
{
    public class CreateOrderItemDto
    {
        [Required(ErrorMessage = "Mã sản phẩm là bắt buộc.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã sản phẩm phải là một số dương.")]
        public int RacketId { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int Quantity { get; set; }
    }
}
