namespace PadelMatch.Models.Requests
{
    public class UpdateUserRequest
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int SkillLevelId { get; set; }
        public string Password { get; set; }
    }
}
