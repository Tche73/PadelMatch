using Domain.Entities;

namespace Domain.Interface.Repositories
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        IEnumerable<Reservation> GetByCourtId(int courtId);
        IEnumerable<Reservation> GetByDateRange(DateTime start, DateTime end);
        IEnumerable<Reservation> GetByUser(int userId);
    }
}
