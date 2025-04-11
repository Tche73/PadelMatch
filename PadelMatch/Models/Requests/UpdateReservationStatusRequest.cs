using Domain.Enums;

namespace PadelMatch.Models.Requests
{
    public class UpdateReservationStatusRequest
    {
        public ReservationStatus Status { get; set; }
    }
}
