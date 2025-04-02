using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IReservationService
    {
        Reservation GetById(int id);
        IEnumerable<Reservation> GetByCourtId(int courtId);
        IEnumerable<Reservation> GetByUserId(int userId);
        IEnumerable<Reservation> GetByDateRange(DateTime start, DateTime end);
        IEnumerable<Reservation> GetAvailableTimeSlots(int courtId, DateTime date);
        void Create(Reservation reservation);
        void Update(Reservation reservation);
        void Cancel(int id);
        void ChangeStatus(int id, ReservationStatus status);
    }
}
