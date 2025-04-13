using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface ICourtRepository : IRepository<Court>
    {
        IEnumerable<Court> GetAvailableCourts(DateTime date, TimeSpan startTime, TimeSpan endTime);
    }
}