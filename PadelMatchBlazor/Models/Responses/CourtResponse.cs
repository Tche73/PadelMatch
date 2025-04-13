namespace PadelMatchBlazor.Models.Responses
{
    public class CourtResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsIndoor { get; set; }
        public decimal PricePerHour { get; set; }
    }
}
