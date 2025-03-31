using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int SkillLevelId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        // Propriétés de navigation
        public virtual SkillLevel SkillLevel { get; set; }
        public virtual ICollection<Availability> Availabilities { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<MatchPlayer> MatchPlayers { get; set; }
        public virtual PlayerStats PlayerStats { get; set; }
    }
}
