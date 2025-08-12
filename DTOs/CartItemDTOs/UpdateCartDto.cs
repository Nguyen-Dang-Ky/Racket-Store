using System.ComponentModel.DataAnnotations;

namespace Serberus_Racket_Store.DTOs.CartItemDTOs
{
    public class UpdateCartDto
    {
        [Required(ErrorMessage = "Số lượng là bắt buộc.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int Quantity { get; set; }
    }
}
