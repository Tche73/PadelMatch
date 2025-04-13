using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMatchRepository : IRepository<Match>
    {
        IEnumerable<Match> GetByUserId(int userId);
        IEnumerable<User> GetMatchPlayers(int matchid);
        User GetPartners(int userid, int matchid);
        IEnumerable<User> GetOpponents(int userid, int matchid);
    }
}
