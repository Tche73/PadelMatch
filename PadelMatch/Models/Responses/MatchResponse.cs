using Domain.Enums;

namespace PadelMatch.Models.Responses
{
    public class MatchResponse
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string CourtName { get; set; }
        public DateTime CreatedAt { get; set; }
        public MatchStatus Status { get; set; }
        public List<MatchPlayerResponse> Players { get; set; }  
        public List<MatchPlayerResponse> Team1Players { get; set; }
        public List<MatchPlayerResponse> Team2Players { get; set; }
        public int CreatedBy { get; set; }
        public string CreatorUsername { get; set; }
    }
}
