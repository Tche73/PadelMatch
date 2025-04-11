using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructures.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class AvailabilityRepository : IAvailabilityRepository
    {
        private readonly PadelMatchDbContext _context;

        public AvailabilityRepository(PadelMatchDbContext context)
        {
            _context = context;
        }

        public Availability GetById(int id)
        {
            return _context.Availabilities
                .Include(a => a.User)
                .FirstOrDefault(a => a.Id == id);
        }


        public IEnumerable<Availability> GetByUserId(int userId)
        {
            return _context.Availabilities
                .Include(a => a.User)
                .Where(a => a.UserId == userId)
                .ToList();
        }

        public void Add(Availability availability)
        {
            _context.Availabilities.Add(availability);
        }

        public void Update(Availability availability)
        {
            _context.Availabilities.Update(availability);
        }

        public void Delete(int id)
        {
            var availability = _context.Availabilities.Find(id);
            if (availability != null)
            {
                _context.Availabilities.Remove(availability);
            }
        }

        public IEnumerable<Availability> GetAll()
        {
            return _context.Availabilities
                .Include(a => a.User)
                .ToList();
        }

        public IEnumerable<Availability> Find(Expression<Func<Availability, bool>> predicate)
        {
            return _context.Availabilities
                 .Where(predicate)
                 .ToList();
        }

        public void AddRange(IEnumerable<Availability> entities)
        {
            _context.Availabilities.AddRange(entities);
        }

        public void Remove(Availability entity)
        {
            _context.Availabilities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<Availability> entities)
        {
            _context.Availabilities.RemoveRange(entities);
        }

        public bool HasOverlappingAvailability(int userId, int dayOfWeek, TimeSpan startTime, TimeSpan endTime, int? excludeId = null)
        {
            var query = _context.Availabilities
                .Where(a => a.UserId == userId && a.DayOfWeek == dayOfWeek);

            if (excludeId.HasValue)
            {
                query = query.Where(a => a.Id != excludeId.Value);
            }

            return query.Any(a => 
            (startTime >= a.StartTime && startTime < a.EndTime) || 
            (endTime > a.StartTime && endTime <= a.EndTime) || 
            (startTime <= a.StartTime && endTime >= a.EndTime));


        }

        public IEnumerable<Availability> GetByDayAndTimeRange(int dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {
            return _context.Availabilities
                .Where(a => a.DayOfWeek == dayOfWeek)
                .Where(a =>
                    (a.StartTime <= endTime && a.EndTime >= startTime))
                .Include(a => a.User)
                .ToList();
        }
    }
}
