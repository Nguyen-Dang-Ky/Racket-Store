namespace Serberus_Racket_Store.DTOs.OrderItemDTOs
{
    public class OrderItemDto
    {
        public int OrderItemId { get; set; }
        public string? OrderItemCode { get; set; }
        public int OrderId { get; set; }
        public int RacketId { get; set; }
        public decimal Price { get; set; }
        //public bool isDelete { get; set; }
    }
}
