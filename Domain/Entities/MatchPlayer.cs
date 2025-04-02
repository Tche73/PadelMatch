using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MatchPlayer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Match")]
        public int MatchId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [Range(1, 2)]
        public int Team { get; set; }

        [Required]
        public bool IsConfirmed { get; set; }

        // Propriétés de navigation
        public virtual Match Match { get; set; }
        public virtual User User { get; set; }
    }
}
