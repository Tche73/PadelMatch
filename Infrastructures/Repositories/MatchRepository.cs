using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructures.Data;
using DomainMatch = Domain.Entities.Match; 


namespace Infrastructures.Repositories
{
    public class MatchRepository : Repository<DomainMatch>, IMatchRepository
    {
        public MatchRepository(PadelMatchDbContext context) : base(context)
        {
        }

        public IEnumerable<DomainMatch> GetByUserId(int userId)
        {
            return _context.MatchPlayers
                .Where(mp => mp.UserId == userId)
                .Select(mp => mp.Match)
                .Distinct()
                .ToList();
        }

        public IEnumerable<User> GetMatchPlayers(int matchid)
        {
            return _context.MatchPlayers
                .Where(mp => mp.MatchId == matchid)
                .Select(mp => mp.User)
                .ToList();
        }

        public IEnumerable<User> GetOpponents(int matchId, int userId)
        {
            var playerTeam = _context.MatchPlayers
                .Where(mp => mp.MatchId == matchId && mp.UserId == userId)
                .Select(mp => mp.Team)
                .FirstOrDefault();

            return _context.MatchPlayers
                .Where(mp => mp.MatchId == matchId && mp.Team != playerTeam)
                .Select(mp => mp.User)
                .ToList();
        }


        public User GetPartners(int matchId, int userId)
        {
            var playerTeam = _context.MatchPlayers
                .Where(mp => mp.MatchId == matchId && mp.UserId == userId)
                .Select(mp => mp.Team)
                .FirstOrDefault();

            return _context.MatchPlayers
                .Where(mp => mp.MatchId == matchId && mp.Team == playerTeam && mp.UserId != userId)
                .Select(mp => mp.User)
                .FirstOrDefault();
        }
    }
}
