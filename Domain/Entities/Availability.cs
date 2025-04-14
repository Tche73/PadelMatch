using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Availability
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [Range(1, 7)]
        public int DayOfWeek { get; set; }

        
        public TimeSpan? StartTime { get; set; }

        
        public TimeSpan? EndTime { get; set; }

        [Required]
        public bool IsRecurring { get; set; }

        // Propriété de navigation
        public virtual User User { get; set; }
    }
}
