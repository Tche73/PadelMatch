using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AvailabilityService(IAvailabilityRepository availabilityRepository, IUnitOfWork unitOfWork)
        {            
            _unitOfWork = unitOfWork;
        }

        public Availability GetById(int id)
        {
            return _unitOfWork.Availabilities.GetById(id);
        }

        public IEnumerable<Availability> GetByUserId(int userId)
        {
            return _unitOfWork.Availabilities.GetByUserId(userId);
        }

        public void Create(Availability availability)
        {
            if (!availability.StartTime.HasValue || !availability.EndTime.HasValue)
            {
                throw new InvalidOperationException("Les heures de début et de fin sont obligatoires");
            }

            // Vérifier s'il y a des chevauchements
            if (HasOverlappingAvailability(availability.UserId, availability.DayOfWeek, availability.StartTime, availability.EndTime))
            {
                throw new InvalidOperationException("Cette disponibilité chevauche une disponibilité existante");
            }

            _unitOfWork.Availabilities.Add(availability);
            _unitOfWork.Complete();
        }

        public void Update(Availability availability)
        {
            // Vérifier s'il y a des chevauchements, en excluant cette disponibilité
            if (HasOverlappingAvailability(availability.UserId, availability.DayOfWeek, availability.StartTime, availability.EndTime, availability.Id))
            {
                throw new InvalidOperationException("Cette disponibilité chevauche une disponibilité existante");
            }

            _unitOfWork.Availabilities.Update(availability);
            _unitOfWork.Complete();
        }

        public void Delete(int id)
        {
            _unitOfWork.Availabilities.Delete(id);
            _unitOfWork.Complete();
        }

        public bool HasOverlappingAvailability(int userId, int dayOfWeek, TimeSpan? startTime, TimeSpan? endTime, int? excludeId = null)
        {
            if (!startTime.HasValue || !endTime.HasValue)
                return false;

            return _unitOfWork.Availabilities.HasOverlappingAvailability(userId, dayOfWeek, startTime.Value, endTime.Value, excludeId);

        }
    }
}
