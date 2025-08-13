using System.ComponentModel.DataAnnotations;

namespace Serberus_Racket_Store.DTOs.OrderDTOs
{
    public class UpdateOrderDto
    {
        [StringLength(50, ErrorMessage = "Trạng thái không được quá 50 ký tự.")]
        public string? Status { get; set; }
    }
}
