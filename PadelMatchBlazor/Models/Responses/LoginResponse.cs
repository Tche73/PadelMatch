namespace PadelMatchBlazor.Models.Responses
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
        public DateTime Expiration { get; set; }
    }
}