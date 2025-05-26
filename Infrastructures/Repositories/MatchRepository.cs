using Domain.Entities;
using Domain.Interfaces;
using Infrastructures.Data;
using Microsoft.EntityFrameworkCore;
using DomainMatch = Domain.Entities.Match;


namespace Infrastructures.Repositories
{
    public class MatchRepository : Repository<DomainMatch>, IMatchRepository
    {
        public MatchRepository(PadelMatchDbContext context) : base(context)
        {
        }

        public DomainMatch GetByIdWithPlayers(int id)
        {
            return _context.Set<DomainMatch>()
            .Include(m => m.MatchPlayers)
                .ThenInclude(mp => mp.User)
            .FirstOrDefault(m => m.Id == id);
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

        public IEnumerable<DomainMatch> GetAllWithPlayers()
        {
            return _context.Set<DomainMatch>()
                .Include(m => m.MatchPlayers)
                    .ThenInclude(mp => mp.User)
                .Include(m => m.Creator)
                .Include(m => m.Reservation)
                    .ThenInclude(r => r.Court)
                .ToList();
        }

        public IEnumerable<DomainMatch> GetMatchesWithPlayers(List<int> matchIds)
        {
            return _context.Matches
                .Where(m => matchIds.Contains(m.Id))
                .Include(m => m.MatchPlayers)
                    .ThenInclude(mp => mp.User)
                .Include(m => m.Creator)
                .Include(m => m.Reservation)
                    .ThenInclude(r => r.Court)
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
