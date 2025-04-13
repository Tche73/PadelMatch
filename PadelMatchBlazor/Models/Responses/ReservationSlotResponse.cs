using Domain.Enums;

namespace PadelMatchBlazor.Models.Responses
{
    public class ReservationSlotResponse
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSlotStatus Status { get; set; }
    }
}
