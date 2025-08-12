using System.ComponentModel.DataAnnotations;

namespace Serberus_Racket_Store.Models

{
    public class Users
    {
        [Key]
        public int userId { get; set; }
        public string? userCode { get; set; }

        [Required]
        public string? fullName { get; set; }
        public string? email { get; set; }

        [Required]
        public string? passwordHash { get; set; }

        [Required]
        public string? role { get; set; }

        public DateTime createAt { get; set; } = DateTime.UtcNow;

        public bool isDelete { get; set; } = false;

        public virtual ICollection<Reviews>? Reviews { get; set; }
        public virtual ICollection<CartItems>? CartItems { get; set; }  
        public virtual ICollection<Orders>? Orders { get; set; }
    }
}
