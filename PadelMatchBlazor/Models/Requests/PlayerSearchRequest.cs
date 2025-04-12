namespace PadelMatchBlazor.Models.Requests
{
    public class PlayerSearchRequest
    {
        public int? CurrentUserId { get; set; }
        public int? SkillLevelId { get; set; }
        public int? SkillLevelTolerance { get; set; } = 1;
        public int? DayOfWeek { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }

}
