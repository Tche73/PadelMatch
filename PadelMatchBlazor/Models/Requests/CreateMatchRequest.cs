namespace PadelMatchBlazor.Models.Requests
{
    public class CreateMatchRequest
    {
        public int ReservationId { get; set; }
        public List<int> PlayerIds { get; set; }
        public int CreatedBy { get; set; }
    }
}
