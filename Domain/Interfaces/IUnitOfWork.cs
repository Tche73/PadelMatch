using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRepository<SkillLevel> SkillLevels { get; }
        IAvailabilityRepository Availabilities { get; }
        ICourtRepository Courts { get; }
        IReservationRepository Reservations { get; }
        IMatchRepository Matches { get; }
        IRepository<MatchPlayer> MatchPlayers { get; }
        IPlayerStatsRepository PlayerStats { get; }
        int Complete();
    }
}
