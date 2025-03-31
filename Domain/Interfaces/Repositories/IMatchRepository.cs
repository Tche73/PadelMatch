using Domain.Entities;
using Domain.Interface.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IMatchRepository : IRepository<Match>
    {
        IEnumerable<Match> GetByUserId (int userId);
        IEnumerable<User> GetMatchPlayers (int matchid);
        User GetPartners (int userid, int matchid);
        IEnumerable<User> GetOpponents(int userid, int matchid);
    }
}
