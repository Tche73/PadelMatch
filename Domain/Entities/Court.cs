using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Court
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsIndoor { get; set; }
        public bool IsActive { get; set; }

        // Propriété de navigation
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
