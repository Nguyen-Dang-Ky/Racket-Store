namespace Serberus_Racket_Store.DTOs.RacketDTOs
{
    public class RacketDto
    {
        public int RacketId { get; set; }
        public string? RacketCode { get; set; }
        public string? RacketName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ImageURL { get; set; }

        public int BrandId { get; set; }
        public string? BrandName { get; set; }
    }
}
