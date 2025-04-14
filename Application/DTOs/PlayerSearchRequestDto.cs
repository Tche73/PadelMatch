using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO_s
{
    public class PlayerSearchRequestDto
    {
        public int? SkillLevelId { get; set; }
        public int? SkillLevelTolerance { get; set; } = 1;
        public int? DayOfWeek { get; set; } // 1-7 pour lundi à dimanche
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public bool? IsRecurring { get; set; }
        public int? CurrentUserId { get; set; } // Facultatif, pour exclure l'utilisateur actuel
    }
}
