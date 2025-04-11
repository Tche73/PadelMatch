namespace PadelMatchBlazor.Models
{
    public class CourtAvailabilityResponse
    {
        public int CourtId { get; set; }
        public string CourtName { get; set; }
        public List<ReservationSlotResponse> AvailableTimeSlots { get; set; } = new List<ReservationSlotResponse>();
    }
}
