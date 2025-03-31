using Domain.Entities;
using Domain.Interface.Repositories;
using Domain.Interfaces.Repositories;
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
        IRepository<Availability> Availabilities { get; }
        IRepository<Court> Courts { get; }
        IReservationRepository Reservations { get; }
        IMatchRepository Matches { get; }
        IRepository<MatchPlayer> MatchPlayers { get; }
        IRepository<PlayerStats> PlayerStats { get; }

        int Complete();
    }
}
