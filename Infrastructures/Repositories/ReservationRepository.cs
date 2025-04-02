using Domain.Entities;
using Domain.Interface.Repositories;
using Infrastructures.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        public ReservationRepository(PadelMatchDbContext context) : base(context)
        {
        }

        public IEnumerable<Reservation> GetByCourtId(int courtId)
        {
           return _dbSet.Where(r => r.CourtId == courtId).ToList();
        }

        public IEnumerable<Reservation> GetByDateRange(DateTime start, DateTime end)
        {
           return _dbSet.Where(r => r.StartDateTime >= start && r.EndDateTime <= end).ToList();
        }

        public IEnumerable<Reservation> GetByUser(int userId)
        {
            return _dbSet.Where(r => r.CreatedBy == userId).ToList();
        }
    }
}
