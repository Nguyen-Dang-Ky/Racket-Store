using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Serberus_Racket_Store.Models
{
    public class Rackets
    {
        [Key]
        public int racketId { get; set; }
        public string? racketCode { get; set; }

        public int brandId { get; set; }
        [ForeignKey("brandId")]
        [JsonIgnore]
        public virtual Brands? Brands { get; set; }

        public string? racketName { get; set; }
        public string? type { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public string? description { get; set; }
        public string? imageURL { get; set; }
        public DateTime createAt { get; set; }
        public bool isDelete { get; set; } = false;
        public virtual ICollection<Reviews>? Reviews { get; set; }
        public virtual ICollection<CartItems>? CartItems { get; set; }
        public virtual ICollection<OrderItems>? OrderItems { get; set; }
    }
}
