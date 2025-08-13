using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Serberus_Racket_Store.Models
{
    public class OrderItems
    {
        [Key]
        public int orderItemId { get; set; }
        public string? orderItemCode { get; set; }        

        public int orderId { get; set; }
        [ForeignKey("orderId")]
        [JsonIgnore]
        public virtual Orders? Orders { get; set; }

        public int racketId { get; set; }
        [ForeignKey("racketId")]
        [JsonIgnore]
        public virtual Rackets? Rackets { get; set; }


        public int quantity { get; set; }
        public decimal price { get; set; }
        public bool isDelete { get; set; } = false;
    }
}
