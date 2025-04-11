using BStorm.Tools.Database;
using Domain.Entities;
using Domain.Interface.Repositories;
using Infrastructures.Data;
using Microsoft.Data.SqlClient;

namespace Infrastructures.Repositories
{
    public class PlayerStatsRepository : Repository<PlayerStats>, IPlayerStatsRepository
    {        
        public PlayerStatsRepository(PadelMatchDbContext context) : base(context)
        {
        }

        public void UpdatePlayerStats(int userId, bool isWin, DateTime? matchDate = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.ExecuteNonQuery(
                    "sp_UpdatePlayerStats",
                    isStoredProcedure: true,
                    parameters: new
                    {
                        UserId = userId,
                        IsWin = isWin,
                        MatchDate = matchDate
                    }
                );
            }
        }
    }
}