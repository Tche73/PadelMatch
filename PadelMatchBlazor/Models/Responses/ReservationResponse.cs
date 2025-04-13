using Domain.Entities;
using PadelMatchBlazor.Models.Enums;

namespace PadelMatchBlazor.Models.Responses
{
    public class ReservationResponse : ApiResponse<Reservation>
    {
        public int Id { get; set; }
        public int CourtId { get; set; }
        public string CourtName { get; set; } = string.Empty;
        public int CreatedBy { get; set; }
        public string CreatorName { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public ReservationStatus Status { get; set; }
    }
}
