using Domain.Entities;
using System;

namespace Domain.Interfaces
{
    public interface IPlayerStatsRepository : IRepository<PlayerStats>
    {
        void UpdatePlayerStats(int userId, bool isWin, DateTime? matchDate = null);
    }
}