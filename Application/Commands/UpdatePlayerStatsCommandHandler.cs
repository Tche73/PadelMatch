using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BStorm.Tools.Database;
using Microsoft.Data.SqlClient;

namespace Application.Commands
{
    public class UpdatePlayerStatsCommandHandler
    {
        private readonly string _connectionString;

        public UpdatePlayerStatsCommandHandler(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Handle(UpdatePlayerStatsCommand command)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                connection.ExecuteNonQuery(
                    "sp_UpdatePlayerStats",
                    isStoredProcedure: true,
                    parameters: new
                    {
                        UserId = command.UserId,
                        IsWin = command.IsWin,
                        MatchDate = command.MatchDate ?? (object)DBNull.Value
                    });
            }
        }
    }
}