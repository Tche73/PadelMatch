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
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Court")]
        public int CourtId { get; set; }

        [Required]
        [ForeignKey("Creator")]
        public int CreatedBy { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public ReservationStatus Status { get; set; }

        // Propriétés de navigation
        public virtual Court Court { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User Creator { get; set; }

        public virtual Match Match { get; set; }
    }
}
