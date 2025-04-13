namespace PadelMatch.Models.Requests
{
    public class UpdateAvailabilityRequest
    {
        public int DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsRecurring { get; set; }
    }
}
