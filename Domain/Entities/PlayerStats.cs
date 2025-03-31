using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PlayerStats
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TotalMatches { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public decimal? WinRate { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Propriété de navigation
        public virtual User User { get; set; }
    }
}
