using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum MatchStatus
    {
        /// <summary>
        /// Match créé mais pas encore confirmé par tous les joueurs
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Match confirmé par tous les joueurs et programmé
        /// </summary>
        Confirmed = 2,

        /// <summary>
        /// Match en cours de jeu
        /// </summary>
        InProgress = 3,

        /// <summary>
        /// Match terminé avec un résultat
        /// </summary>
        Completed = 4,

        /// <summary>
        /// Match annulé avant qu'il ne commence
        /// </summary>
        Cancelled = 5,

        /// <summary>
        /// Match non joué car au moins un joueur ne s'est pas présenté
        /// </summary>
        NoShow = 6,

        /// <summary>
        /// Match reporté à une date ultérieure
        /// </summary>
        Postponed = 7
    }
}
