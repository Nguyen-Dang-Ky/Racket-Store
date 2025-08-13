
namespace Serberus_Racket_Store.DTOs.UserDTOs
{
    public class CreateUserDto
    {
        public string fullName { get; set; } = null!;
        public string passwordHash { get; set; } = null!;   
        public string email { get; set; } = null!;
        public string role { get; set; } = "User";
    }
}
