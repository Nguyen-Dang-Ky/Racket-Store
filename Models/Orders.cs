using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text.Json.Serialization;

namespace Serberus_Racket_Store.Models
{
    public class Orders
    {
        [Key]
        public int orderId { get; set; }
        public string? orderCode { get; set; }

        public int userId { get; set; }
        [ForeignKey("userId")]
        [JsonIgnore]
        public virtual Users? Users { get; set; }

        public DateTime orderDate { get; set; }
        public decimal total { get; set; }
        public string? status { get; set; }
        public bool isDelete { get; set; } = false;
        public virtual Shipping_Info Shipping_Info { get; set; }
        public virtual ICollection<OrderItems>? OrderItems { get; set; }
    }
}
