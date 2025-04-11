using Domain.Enums;

namespace PadelMatch.Models.Responses
{
    public class ReservationResponse
    {
        public int Id { get; set; }
        public int CourtId { get; set; }
        public string CourtName { get; set; }
        public int CreatedBy { get; set; }
        public string CreatorName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public ReservationStatus Status { get; set; }
    }
}
