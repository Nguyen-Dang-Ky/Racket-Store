namespace Serberus_Racket_Store.DTOs.ShippingInfoDTOs
{
    public class ShippingInfoDto
    {
        public int ShippingId { get; set; }
        public string? ShippingCode { get; set; }
        public int OrderId { get; set; } 
        public string? ReceiverName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? ShippingMethod { get; set; }
        public decimal ShippingFee { get; set; }
    }
}
