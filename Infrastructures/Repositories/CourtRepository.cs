using BStorm.Tools.Database;
using Domain.Entities;
using Domain.Interface.Repositories;
using Infrastructures.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class CourtRepository : Repository<Court>, ICourtRepository
    {


        public CourtRepository(PadelMatchDbContext context) : base(context)
        {
        }

        public IEnumerable<Court> GetAvailableCourts(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.ExecuteReader(
                    "sp_GetAvailableCourts",
                    record => new Court
                    {
                        Id = (int)record["Id"],
                        Name = (string)record["Name"],
                        IsIndoor = (bool)record["IsIndoor"],
                        IsActive = (bool)record["IsActive"]
                    },
                    isStoredProcedure: true,
                    parameters: new
                    {
                        Date = date,
                        StartTime = startTime,
                        EndTime = endTime
                    }
                );
            }
        }

        public override IEnumerable<Court> GetAll()
        {
            return _dbSet.ToList();
        }
    }
}