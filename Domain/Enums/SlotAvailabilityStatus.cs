using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum SlotAvailabilityStatus
    {
        /// <summary>
        /// Le créneau est disponible pour réservation
        /// </summary>
        Available = 1,

        /// <summary>
        /// Le créneau est réservé
        /// </summary>
        Booked = 2,
        
        /// <summary>
        /// Le créneau est en maintenance ou indisponible
        /// </summary>
        Unavailable = 3
    }
}
