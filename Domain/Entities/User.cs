using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [ForeignKey("SkillLevel")]
        public int SkillLevelId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public bool IsActive { get; set; }

        // Navigation properties
        public virtual SkillLevel SkillLevel { get; set; }
        public virtual ICollection<Availability> Availabilities { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<MatchPlayer> MatchPlayers { get; set; }
        public virtual PlayerStats PlayerStats { get; set; }
    }
}
