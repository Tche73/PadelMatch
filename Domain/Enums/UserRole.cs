using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum UserRole
    {
        /// <summary>
        /// Utilisateur standard avec droits limités
        /// </summary>
        User = 1,

        /// <summary>
        /// Administrateur avec tous les droits
        /// </summary>
        Admin = 2
    }
}
