using Domain.Enums;

namespace PadelMatchBlazor.Models
{
    public class ReservationSlotResponse
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ReservationStatus Status { get; set; }
    }
}
