namespace PadelMatch.Models.Responses
{
    public class MatchPlayerResponse
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int Team { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsOrganizer { get; set; }
        public string FirstName { get; set; }        
        public string LastName { get; set; }        
    }
}
