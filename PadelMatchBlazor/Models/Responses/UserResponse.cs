namespace PadelMatchBlazor.Models.Responses
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
        public DateTime CreatedAt { get; set; }
        // Nouvelle propriété pour les disponibilités
        public IEnumerable<AvailabilityResponse> Availabilities { get; set; } = new List<AvailabilityResponse>();
    }

}
