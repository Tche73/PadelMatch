namespace PadelMatchBlazor.Models.Requests
{
    public class CreateReservationRequest
    {
        public int CourtId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
