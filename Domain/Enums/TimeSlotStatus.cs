using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    /// <summary>
    /// Représente l'état de disponibilité d'un créneau horaire
    /// </summary>
    public enum TimeSlotStatus
    {

        /// <summary>
        /// Le créneau est disponible pour réservation
        /// </summary>
        Available = 1,

        /// <summary>
        /// Le créneau est déjà réservé
        /// </summary>
        Booked = 2,

        /// <summary>
        /// Le créneau est indisponible (maintenance, etc.)
        /// </summary>
        Unavailable = 3
    }
}
