using BStorm.Tools.Database;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructures.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        public ReservationRepository(PadelMatchDbContext context) : base(context)
        {
        }

        public IEnumerable<Reservation> GetByCourtId(int courtId)
        {
            //return _dbSet.Where(r => r.CourtId == courtId).ToList();
            return _context.Reservations
                .Include(r => r.Court)
                .Include(r => r.Creator)
                .Where(r => r.CourtId == courtId)
                .ToList();

        }

        public IEnumerable<Reservation> GetByDateRange(DateTime start, DateTime end)
        {
           return _dbSet.Where(r => r.StartDateTime >= start && r.EndDateTime <= end).ToList();
        }

        public IEnumerable<Reservation> GetByUser(int userId)
        {
            //return _dbSet.Where(r => r.CreatedBy == userId).ToList();
            return _context.Reservations
            .Include(r => r.Court)
            .Include(r => r.Creator)
            .Where(r => r.CreatedBy == userId)
            .ToList();
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
        public override IEnumerable<Reservation> GetAll()
        {
            return _context.Reservations
                .Include(r => r.Court)
                .Include(r => r.Creator)
                .ToList();
        }
        public override Reservation GetById(int id)
        {
            return _context.Reservations
                .Include(r => r.Court)
                .Include(r => r.Creator)
                .FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Reservation> GetReservationsForDate(DateTime date)
        {
            return _context.Reservations
                 .Include(r => r.Court)
                 .Include(r => r.Creator)
                 .Where(r => r.StartDateTime.Date == date)
                 .ToList();
        }

        public IEnumerable<Reservation> GetReservationsByUserId(int userId)
        {
            //return _dbSet.Where(r => r.CreatedBy == userId).ToList();
            return _context.Reservations
            .Include(r => r.Court)
            .Include(r => r.Creator)
            .Where(r => r.CreatedBy == userId)
            .ToList();
        }
    }
}
