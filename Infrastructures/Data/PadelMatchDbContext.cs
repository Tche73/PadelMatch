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

            // Index pour les recherches par nom d'utilisateur et email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configuration des comportements de suppression
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Court)
                .WithMany(c => c.Reservations)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Creator)
                .WithMany(u => u.Reservations)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Reservation)
                .WithOne(r => r.Match)
                .HasForeignKey<Match>(m => m.ReservationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index pour optimiser les performances des requêtes fréquentes
            modelBuilder.Entity<Reservation>()
                .HasIndex(r => r.StartDateTime);

            modelBuilder.Entity<Reservation>()
                .HasIndex(r => r.Status);

            modelBuilder.Entity<Match>()
                .HasIndex(m => m.Status);
        }
    }    
}
