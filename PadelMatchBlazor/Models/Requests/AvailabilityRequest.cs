namespace PadelMatchBlazor.Models.Requests
{
    public class AvailabilityRequest
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsRecurring { get; set; } = false; // Ajout de la propriété avec une valeur par défaut
    }

}

