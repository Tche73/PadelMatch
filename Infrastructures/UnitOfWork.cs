using Domain.Entities;
using Domain.Interface.Repositories;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Infrastructures.Data;
using Infrastructures.Repositories;

namespace Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PadelMatchDbContext _context;



        public IUserRepository Users { get; private set; }

        public IRepository<SkillLevel> SkillLevels { get; private set; }

        public IRepository<Availability> Availabilities { get; private set; }

        public IRepository<Court> Courts { get; private set; }

        public IReservationRepository Reservations { get; private set; }

        public IMatchRepository Matches { get; private set; }

        public IRepository<MatchPlayer> MatchPlayers { get; private set; }

        public IRepository<PlayerStats> PlayerStats { get; private set; }

        public UnitOfWork(PadelMatchDbContext context)
        {
            _context = context;

            Users = new UserRepository(_context);
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
