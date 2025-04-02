using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Match
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Reservation")]
        public int ReservationId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public MatchStatus Status { get; set; }

        // Propriétés de navigation
        public virtual Reservation Reservation { get; set; }
        public virtual ICollection<MatchPlayer> MatchPlayers { get; set; }
    }
}
