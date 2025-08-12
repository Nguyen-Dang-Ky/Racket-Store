using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Serberus_Racket_Store.Models
{
    public class Reviews
    {
        [Key]
        public int reviewId { get; set; }
        public string? reviewCode { get; set; }        
        public int userId {  get; set; }
        [ForeignKey("userId")]
        [JsonIgnore]
        public virtual Users? Users { get; set; }
         
        public int racketId { get; set;}
        [ForeignKey("racketId")]
        [JsonIgnore]
        public virtual Rackets? Rackets { get; set; }

        public int rating { get; set; }
        public string? comment { get; set; }
        public DateTime createAt { get; set; }
        public bool isDelete { get; set; } = false;
    }
}
