using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Match
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public MatchStatus Status { get; set; }

        // Propriétés de navigation
        public virtual Reservation Reservation { get; set; }
        public virtual ICollection<MatchPlayer> MatchPlayers { get; set; }
    }
}
