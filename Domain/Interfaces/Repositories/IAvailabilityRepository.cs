using Domain.Entities;
using Domain.Interface.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IAvailabilityRepository : IRepository<Availability>
    {
        Availability GetById(int id);
        IEnumerable<Availability> GetByUserId(int userId);
        void Add(Availability availability);
        void Update(Availability availability);
        void Delete(int id);
        bool HasOverlappingAvailability(int userId, int dayOfWeek, TimeSpan startTime, TimeSpan endTime, int? excludeId = null);
        IEnumerable<Availability> GetByDayAndTimeRange(int dayOfWeek, TimeSpan startTime, TimeSpan endTime);
    }
}
