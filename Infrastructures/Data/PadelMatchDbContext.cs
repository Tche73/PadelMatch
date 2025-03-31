using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Data
{
    public class PadelMatchDbContext : DbContext
    {
        public PadelMatchDbContext(DbContextOptions<PadelMatchDbContext> options) 
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SkillLevel> SkillLevels { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Court> Courts { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchPlayer> MatchPlayers { get; set; }
        public DbSet<PlayerStats> PlayerStats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurations des entités seront ajoutées ici
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PadelMatchDbContext).Assembly);
        }
    }
}
