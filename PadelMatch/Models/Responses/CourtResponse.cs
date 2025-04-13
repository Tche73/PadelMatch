namespace PadelMatch.Models.Responses
{
    public class CourtResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsIndoor { get; set; }
        public bool IsActive { get; set; }
    }
}