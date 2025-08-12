using System.ComponentModel.DataAnnotations;

namespace Serberus_Racket_Store.Models
{
    public class Brands
    {
        [Key]
        public int brandId { get; set; }
        public string? brandCode { get; set; }
        public string brandName { get; set; }
        public bool isDelete { get; set; }  = false;
        public virtual ICollection<Rackets>? Rackets {  get; set; }
    }
}
