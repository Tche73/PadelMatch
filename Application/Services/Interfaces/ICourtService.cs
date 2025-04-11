using Application.DTO_s;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ICourtService
    {
        IEnumerable<Court> GetAvailableCourts(DateTime date, TimeSpan startTime, TimeSpan endTime);

        IEnumerable<Court> GetAllCourts();
        Court GetCourtById(int id);

        // Méthode pour obtenir les disponibilités détaillées des terrains pour une date donnée
        IEnumerable<CourtAvailabilityDetail> GetCourtsAvailabilityForDate(DateTime date);
    }
}
