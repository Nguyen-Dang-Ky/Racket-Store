namespace Serberus_Racket_Store.DTOs.UserDTOs
{
    public class LoginResponseDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
