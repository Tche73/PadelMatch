using Domain.Entities;
using Domain.Interface.Repositories;

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
