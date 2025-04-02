using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PlayerStats
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        public int TotalMatches { get; set; } = 0;

        [Required]
        public int Wins { get; set; } = 0;

        [Required]
        public int Losses { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        public decimal? WinRate { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        // Propriété de navigation
        public virtual User User { get; set; }
    }
}
