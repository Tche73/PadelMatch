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

        public IAvailabilityRepository Availabilities { get; private set; }

        public ICourtRepository Courts { get; private set; }

        public IReservationRepository Reservations { get; private set; }

        public IMatchRepository Matches { get; private set; }

        public IRepository<MatchPlayer> MatchPlayers { get; private set; }
     
        public IPlayerStatsRepository PlayerStats { get; private set; }

        public UnitOfWork(PadelMatchDbContext context)
        {
            _context = context;

            Users = new UserRepository(_context);
            SkillLevels = new Repository<SkillLevel>(_context);
            Availabilities = new AvailabilityRepository(_context);
            Courts = new CourtRepository(_context);
            Reservations = new ReservationRepository(_context);
            Matches = new MatchRepository(_context);
            MatchPlayers = new Repository<MatchPlayer>(_context);
            PlayerStats = new PlayerStatsRepository(_context);

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
