using Application.DTO_s;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class CourtService : ICourtService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourtService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Court> GetAllCourts()
        {
            return _unitOfWork.Courts.GetAll();
        }

        public IEnumerable<Court> GetAvailableCourts(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            return _unitOfWork.Courts.GetAvailableCourts(date, startTime, endTime);
        }

        public Court GetCourtById(int id)
        {
            return _unitOfWork.Courts.GetById(id);
        }

        public IEnumerable<CourtAvailabilityDetail> GetCourtsAvailabilityForDate(DateTime date)
        {
            var courts = _unitOfWork.Courts.GetAll().Where(c => c.IsActive).ToList();
            var result = new List<CourtAvailabilityDetail>();

            // Définir les créneaux horaires standard (par exemple, de 8h à 22h)
            var openingTime = new TimeSpan(8, 0, 0);
            var closingTime = new TimeSpan(22, 0, 0);
            var slotDuration = TimeSpan.FromHours(1);

            // Récupérer toutes les réservations pour la date donnée
            var reservations = _unitOfWork.Reservations.GetReservationsForDate(date);

            foreach (var court in courts)
            {
                var courtDetail = new CourtAvailabilityDetail
                {
                    Court = court,
                    AvailableTimeSlots = new List<TimeSlot>()
                };

                // Générer tous les créneaux horaires pour la journée
                for (var time = openingTime; time < closingTime; time = time.Add(slotDuration))
                {
                    var startDateTime = date.Date.Add(time);
                    var endDateTime = startDateTime.Add(slotDuration);

                    // Vérifier si ce créneau est réservé
                    var isBooked = reservations.Any(r =>
                        r.CourtId == court.Id &&
                        r.StartDateTime < endDateTime &&
                        r.EndDateTime > startDateTime);

                    var status = isBooked ? SlotAvailabilityStatus.Booked : SlotAvailabilityStatus.Available;

                    courtDetail.AvailableTimeSlots.Add(new TimeSlot
                    {
                        StartTime = startDateTime,
                        EndTime = endDateTime,
                        Status = status
                    });
                }

                result.Add(courtDetail);
            }

            return result;
        }
    }
}
