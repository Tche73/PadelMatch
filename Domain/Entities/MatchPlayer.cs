using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MatchPlayer
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int UserId { get; set; }
        public int Team { get; set; }
        public bool IsConfirmed { get; set; }

        // Propriétés de navigation
        public virtual Match Match { get; set; }
        public virtual User User { get; set; }
    }
}
