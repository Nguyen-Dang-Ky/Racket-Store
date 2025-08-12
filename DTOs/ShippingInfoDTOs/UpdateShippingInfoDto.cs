using System.ComponentModel.DataAnnotations;

namespace Serberus_Racket_Store.DTOs.ShippingInfoDTOs
{
    public class UpdateShippingInfoDto
    {
        [StringLength(100, ErrorMessage = "Tên người nhận không được quá 100 ký tự.")]
        public string? ReceiverName { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [StringLength(20, ErrorMessage = "Số điện thoại không được quá 20 ký tự.")]
        public string? PhoneNumber { get; set; }

        [StringLength(200, ErrorMessage = "Địa chỉ không được quá 200 ký tự.")]
        public string? Address { get; set; }

        [StringLength(50, ErrorMessage = "Phương thức vận chuyển không được quá 50 ký tự.")]
        public string? ShippingMethod { get; set; }
    }
}
