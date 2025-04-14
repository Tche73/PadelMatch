using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        IEnumerable<Reservation> GetByCourtId(int courtId);
        IEnumerable<Reservation> GetByDateRange(DateTime start, DateTime end);
        IEnumerable<Reservation> GetReservationsForDate(DateTime date);
        IEnumerable<Reservation> GetByUserId(int userId);
       
    }
}
