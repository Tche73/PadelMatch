using Domain.Enums;

namespace PadelMatch.Models.Requests
{
    public class RegisterUserRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int SkillLevelId { get; set; }               
    }
}
