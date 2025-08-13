using Serberus_Racket_Store.DTOs.OrderItemDTOs;
using Serberus_Racket_Store.DTOs.ShippingInfoDTOs;

namespace Serberus_Racket_Store.DTOs.OrderDTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string? OrderCode { get; set; }
        public int UserId { get; set; } // Only the ID, not the full User object
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public string? Status { get; set; }
        public ShippingInfoDto? Shipping_Info { get; set; } // If you want shipping info directly in the order
        public ICollection<OrderItemDto>? OrderItems { get; set; }

    }
}
