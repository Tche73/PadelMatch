using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Repositories
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        IEnumerable<Reservation> GetByCourtId(int courtId);
        IEnumerable<Reservation> GetByDateRange(DateTime start, DateTime end);
        IEnumerable<Reservation> GetByUser(int userId);
    }
}
