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
    public class CourtService : ICourtService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourtService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Court> GetAvailableCourts(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            return _unitOfWork.Courts.GetAvailableCourts(date, startTime, endTime);
        }
    }
}
