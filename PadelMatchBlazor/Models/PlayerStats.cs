namespace PadelMatchBlazor.Models
{
    public class PlayerStats
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TotalMatches { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public decimal WinRate { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}