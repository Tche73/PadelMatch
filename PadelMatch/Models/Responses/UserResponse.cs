namespace PadelMatch.Models.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int SkillLevelId { get; set; }
        public string SkillLevelName { get; set; }
        public bool IsActive { get; set; }
    }
}
