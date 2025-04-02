using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class MatchService : IMatchService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MatchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void AddPlayer(int matchId, int userId, int team)
        {
            // Vérifier que le match existe
            var player = _unitOfWork.Matches.GetById(matchId);
            if (player == null)
            {
                throw new InvalidOperationException("Le match spécifié n'existe pas.");
            }

            // Vérifier que le joueur existe
            var user = _unitOfWork.Users.GetById(userId);
            if (user == null)
                throw new InvalidOperationException("Le joueur spécifié n'existe pas.");

            // Vérifier que le joueur n'est pas déjà dans ce match
            var existingPlayer = _unitOfWork.MatchPlayers
                .Find(mp => mp.MatchId == matchId && mp.UserId == userId)
                .FirstOrDefault();

            if (existingPlayer != null)
                throw new InvalidOperationException("Ce joueur est déjà inscrit à ce match.");

            // Vérifier que l'équipe n'est pas complète (maximum 2 joueurs par équipe)
            var playersInTeam = _unitOfWork.MatchPlayers
                .Find(mp => mp.MatchId == matchId && mp.Team == team)
                .Count();

            if (playersInTeam >= 2)
                throw new InvalidOperationException("Cette équipe est déjà complète (2 joueurs maximum).");

            // Ajouter le joueur au match
            var matchPlayer = new MatchPlayer
            {
                MatchId = matchId,
                UserId = userId,
                Team = team,
                IsConfirmed = false // Le joueur doit confirmer sa participation
            };

            _unitOfWork.MatchPlayers.Add(matchPlayer);
            _unitOfWork.Complete();

        }

        public void ChangeStatus(int id, MatchStatus status)
        {
            var match = _unitOfWork.Matches.GetById(id);
            if (match == null)
                throw new InvalidOperationException("Le match spécifié n'existe pas.");

            // Vérifier la logique de transition d'état
            if (match.Status == MatchStatus.Cancelled && status != MatchStatus.Cancelled)
                throw new InvalidOperationException("Impossible de changer le statut d'un match annulé.");

            if (match.Status == MatchStatus.Completed && status != MatchStatus.Completed)
                throw new InvalidOperationException("Impossible de changer le statut d'un match terminé.");

            // Si le match passe à Confirmed, vérifier que tous les joueurs ont confirmé
            if (status == MatchStatus.Confirmed)
            {
                var matchPlayers = _unitOfWork.MatchPlayers.Find(mp => mp.MatchId == id).ToList();
                if (matchPlayers.Count < 4)
                    throw new InvalidOperationException("Le match doit avoir 4 joueurs pour être confirmé.");

                if (matchPlayers.Any(mp => !mp.IsConfirmed))
                    throw new InvalidOperationException("Tous les joueurs doivent confirmer leur participation avant de confirmer le match.");
            }

            match.Status = status;
            _unitOfWork.Complete();
        }

        public void CompleteMatch(int id, List<int> winningTeamUserIds)
        {
            var match = _unitOfWork.Matches.GetById(id);
            if (match == null)
                throw new InvalidOperationException("Le match spécifié n'existe pas.");

            // Vérifier que le match n'est pas déjà terminé ou annulé
            if (match.Status == MatchStatus.Completed || match.Status == MatchStatus.Cancelled)
                throw new InvalidOperationException("Impossible de compléter un match déjà terminé ou annulé.");

            // Vérifier que l'équipe gagnante a exactement 2 joueurs
            if (winningTeamUserIds.Count != 2)
                throw new InvalidOperationException("L'équipe gagnante doit comporter exactement 2 joueurs.");

            // Vérifier que tous les joueurs gagnants font partie du match
            var matchPlayers = _unitOfWork.MatchPlayers.Find(mp => mp.MatchId == id).ToList();
            foreach (var userId in winningTeamUserIds)
            {
                if (!matchPlayers.Any(mp => mp.UserId == userId))
                    throw new InvalidOperationException($"Le joueur avec l'ID {userId} ne participe pas à ce match.");
            }

            // Vérifier que les deux joueurs gagnants sont dans la même équipe
            var teams = matchPlayers
                .Where(mp => winningTeamUserIds.Contains(mp.UserId))
                .Select(mp => mp.Team)
                .ToList();

            if (teams.Distinct().Count() != 1)
                throw new InvalidOperationException("Les joueurs gagnants doivent être dans la même équipe.");

            // Mettre à jour le statut du match
            match.Status = MatchStatus.Completed;
            _unitOfWork.Complete();

            // Mettre à jour les statistiques des joueurs
            var winningTeam = teams.First();
            foreach (var player in matchPlayers)
            {
                var playerStat = _unitOfWork.PlayerStats.Find(ps => ps.UserId == player.UserId).FirstOrDefault();
                if (playerStat == null)
                {
                    // Créer des stats pour ce joueur s'il n'en a pas
                    playerStat = new PlayerStats
                    {
                        UserId = player.UserId,
                        TotalMatches = 0,
                        Wins = 0,
                        Losses = 0,
                        UpdatedAt = DateTime.Now
                    };
                    _unitOfWork.PlayerStats.Add(playerStat);
                }

                // Incrémenter le nombre total de matchs
                playerStat.TotalMatches++;

                // Incrémenter les victoires ou défaites
                if (player.Team == winningTeam)
                {
                    playerStat.Wins++;
                }
                else
                {
                    playerStat.Losses++;
                }

                // Calculer le taux de victoire
                playerStat.WinRate = (decimal)playerStat.Wins / playerStat.TotalMatches * 100;
                playerStat.UpdatedAt = DateTime.Now;
            }

            _unitOfWork.Complete();
        }

        public void Create(Match match, List<int> playerIds)
        {
            // Vérifier que la réservation existe
            var reservation = _unitOfWork.Reservations.GetById(match.ReservationId);
            if (reservation == null)
                throw new InvalidOperationException("La réservation spécifiée n'existe pas.");

            // Vérifier que la réservation n'est pas déjà liée à un match
            if (_unitOfWork.Matches.Find(m => m.ReservationId == match.ReservationId).Any())
                throw new InvalidOperationException("Cette réservation est déjà associée à un match.");

            // Vérifier que nous avons exactement 4 joueurs pour un match de padel
            if (playerIds.Count != 4)
                throw new InvalidOperationException("Un match de padel nécessite exactement 4 joueurs.");

            // Vérifier que tous les joueurs existent
            foreach (var playerId in playerIds)
            {
                var player = _unitOfWork.Users.GetById(playerId);
                if (player == null)
                    throw new InvalidOperationException($"Le joueur avec l'ID {playerId} n'existe pas.");
            }

            // Définir les valeurs par défaut pour le match
            match.CreatedAt = DateTime.Now;
            match.Status = MatchStatus.Pending;

            // Ajouter le match
            _unitOfWork.Matches.Add(match);
            _unitOfWork.Complete();

            // Ajouter les joueurs au match (diviser en 2 équipes)
            for (int i = 0; i < playerIds.Count; i++)
            {
                var team = i < 2 ? 1 : 2; // Équipe 1 pour les 2 premiers joueurs, Équipe 2 pour les 2 autres
                var matchPlayer = new MatchPlayer
                {
                    MatchId = match.Id,
                    UserId = playerIds[i],
                    Team = team,
                    IsConfirmed = false // Par défaut, les joueurs doivent confirmer leur participation
                };

                _unitOfWork.MatchPlayers.Add(matchPlayer);
            }

            _unitOfWork.Complete();
        }

        public Match GetById(int id)
        {
            return _unitOfWork.Matches.GetById(id);
        }

        public IEnumerable<Match> GetByUserId(int userId)
        {
            return _unitOfWork.Matches.GetByUserId(userId);
        }

        public IEnumerable<User> GetOpponents(int matchId, int userId)
        {
            return _unitOfWork.Matches.GetOpponents(matchId, userId);
        }

        public User GetPartner(int matchId, int userId)
        {
            return _unitOfWork.Matches.GetPartners(matchId, userId);
        }

        public void RemovePlayer(int matchId, int userId)
        {
            // Vérifier que le match existe
            var match = _unitOfWork.Matches.GetById(matchId);
            if (match == null)
                throw new InvalidOperationException("Le match spécifié n'existe pas.");

            // Vérifier que le joueur est bien inscrit à ce match
            var matchPlayer = _unitOfWork.MatchPlayers
                .Find(mp => mp.MatchId == matchId && mp.UserId == userId)
                .FirstOrDefault();

            if (matchPlayer == null)
                throw new InvalidOperationException("Ce joueur n'est pas inscrit à ce match.");

            // Empêcher le retrait si le match est déjà en cours ou terminé
            if (match.Status == MatchStatus.InProgress || match.Status == MatchStatus.Completed)
                throw new InvalidOperationException("Impossible de retirer un joueur d'un match en cours ou terminé.");

            // Retirer le joueur du match
            _unitOfWork.MatchPlayers.Remove(matchPlayer);
            _unitOfWork.Complete();
        }
    }
}
