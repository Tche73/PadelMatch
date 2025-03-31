using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum ReservationStatus
    {
        /// <summary>
        /// Réservation créée mais pas encore confirmée
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Réservation confirmée et validée
        /// </summary>
        Confirmed = 2,

        /// <summary>
        /// Réservation annulée par l'utilisateur
        /// </summary>
        Cancelled = 3,

        /// <summary>
        /// Réservation terminée (date passée, terrain utilisé)
        /// </summary>
        Completed = 4,

        /// <summary>
        /// Les joueurs ne se sont pas présentés
        /// </summary>
        NoShow = 5,
    }
}
