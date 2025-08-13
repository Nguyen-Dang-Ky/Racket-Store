using System.ComponentModel.DataAnnotations;

namespace Serberus_Racket_Store.DTOs.CartItemDTOs
{
    public class CreateCartDto
    {
        [Required(ErrorMessage = "Mã người dùng là bắt buộc.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã người dùng phải là một số dương.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Mã vợt là bắt buộc.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã vợt phải là một số dương.")]
        public int RacketId { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int Quantity { get; set; }
    }
}
