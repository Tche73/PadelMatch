namespace PadelMatchBlazor.Models
{
       public class AvailabilityResponse
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public int DayOfWeek { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
            public bool IsRecurring { get; set; }
        }   
    
}
