using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IAvailabilityService
    {
        Availability GetById(int id);
        IEnumerable<Availability> GetByUserId(int userId);
        void Create(Availability availability);
        void Update(Availability availability);
        void Delete(int id);
        bool HasOverlappingAvailability(int userId, int dayOfWeek, TimeSpan startTime, TimeSpan endTime, int? excludeId = null);
    }
}
