using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Serberus_Racket_Store.Models
{
    public class Shipping_Info
    {
        [Key]
        public int shippingId { get; set; }
        public string? shippingCode { get; set; }
        
        public int orderId { get; set; }
        [ForeignKey("orderId")]
        [JsonIgnore]
        public virtual Orders? Orders { get; set; }


        public string? receiverName { get; set; }
        public string? phoneNumber { get; set; }
        public string? address { get; set; }
        public string? shippingMethod { get; set; }
        public decimal shippingFee { get; set; }
        public bool isDelete { get; set; } = false;

    }
}
