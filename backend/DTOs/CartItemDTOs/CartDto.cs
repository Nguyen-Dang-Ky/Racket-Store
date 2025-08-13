namespace Serberus_Racket_Store.DTOs.CartItemDTOs
{
    public class CartDto
    {
        public int CartId { get; set; }
        public string? CartCode { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public int RacketId { get; set; }
        public string? RacketName { get; set; }
        public int Quantity { get; set; }

        public decimal Price { get; set; }
        public string? ImageURL { get; set; }

    }
}
