using Domain.Entities;
using System;

namespace Domain.Interface.Repositories
{
    public interface IPlayerStatsRepository : IRepository<PlayerStats>
    {
        void UpdatePlayerStats(int userId, bool isWin, DateTime? matchDate = null);
    }
}