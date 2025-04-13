namespace PadelMatch.Models.Requests
{
    public class CreateReservationRequest
    {
        public int CourtId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
