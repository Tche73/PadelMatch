using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IMatchService
    {
        Match GetById(int id);
        IEnumerable<Match> GetByUserId(int userId);
        void Create(Match match, List<int> playerIds);
        void AddPlayer(int matchId, int userId, int team);
        void RemovePlayer(int matchId, int userId);
        void ChangeStatus(int id, MatchStatus status);
        void CompleteMatch(int id, List<int> winningTeamUserIds);
        User GetPartner(int matchId, int userId);
        IEnumerable<User> GetOpponents(int matchId, int userId);
    }
}
