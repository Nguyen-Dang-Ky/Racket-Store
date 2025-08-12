using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Serberus_Racket_Store.Models
{
    public class CartItems
    {
        [Key]
        public int cartItemId { get; set; }
        public string? cartItemCode { get; set; }
        public int userId { get; set; }
        [ForeignKey("userId")]
        [JsonIgnore]
        public virtual Users? Users { get; set; }


        public int racketId { get; set; }
        [ForeignKey("racketId")]
        [JsonIgnore]
        public virtual Rackets? Rackets { get; set; }

        public int quantity { get; set; }
        public bool isDelete { get; set; } = false;
    }
}
