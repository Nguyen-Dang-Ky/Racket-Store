namespace Serberus_Racket_Store.DTOs.UserDTOs
{
    public class UserDto
    {
        public int userId { get; set; }
        public string userCode { get; set; } = string.Empty;
        public string fullName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string role { get; set; } = "User";
        public DateTime createdAt { get; set; }
    }
}
