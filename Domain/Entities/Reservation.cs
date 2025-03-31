using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public int CourtId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public ReservationStatus Status { get; set; }

        // Propriétés de navigation
        public virtual Court Court { get; set; }
        public virtual User Creator { get; set; }
        public virtual Match Match { get; set; }
    }
}
