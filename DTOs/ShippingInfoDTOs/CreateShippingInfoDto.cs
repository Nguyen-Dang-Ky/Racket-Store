using System.ComponentModel.DataAnnotations;

namespace Serberus_Racket_Store.DTOs.ShippingInfoDTOs
{
    public class CreateShippingInfoDto
    {
        [Required(ErrorMessage = "User name is require!")]
        [StringLength(100, ErrorMessage = "User name cannot over 100 characters")]
        public string ReceiverName { get; set; }

        [Required(ErrorMessage = "Phone number is require!")]
        [Phone(ErrorMessage = "Phone number is not verify")]
        [StringLength(20, ErrorMessage = "Phone number cannot over 20 charaters")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address is require")]
        [StringLength(200, ErrorMessage = "Address cannot over 200 characters")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Shipping method is require")]
        [StringLength(50, ErrorMessage = "Shipping method cannot over 50 characters")]
        public string ShippingMethod { get; set; } = string.Empty;
    }
}
