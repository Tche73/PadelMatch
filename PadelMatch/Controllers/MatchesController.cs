using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using PadelMatch.Models.Requests;
using PadelMatch.Models.Responses;
using System.Security.Claims;

namespace PadelMatch.Controllers
{
    public class MatchesController : ControllerBase
    {
        private readonly IMatchService _matchService;
        private readonly IReservationService _reservationService;
        private readonly IUserService _userService;

        public MatchesController(IMatchService matchService, IReservationService reservationService, IUserService userService)
        {
            _matchService = matchService;
            _reservationService = reservationService;
            _userService = userService; 
        }

        [HttpGet]
        public ActionResult<IEnumerable<MatchResponse>> GetMatches()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var matches = _matchService.GetByUserId(userId);
            var matchResponses = matches.Select(MapToResponse);
            return Ok(matchResponses);
        }

        [HttpGet("{id}")]
        public ActionResult<MatchResponse> GetMatchById(int id)
        {
            var match = _matchService.GetById(id);
            if (match == null)
                return NotFound();

            return Ok(MapToResponse(match));
        }

        [HttpGet("{id}/partner")]
        public ActionResult<UserResponse> GetPartner(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var partner = _matchService.GetPartner(id, userId);

            if (partner == null)
                return NotFound(new { message = "Partenaire non trouvé" });

            return Ok(new UserResponse
            {
                Id = partner.Id,
                Username = partner.Username,
                Email = partner.Email,
                FirstName = partner.FirstName,
                LastName = partner.LastName,
                SkillLevelId = partner.SkillLevelId,
                SkillLevelName = partner.SkillLevel?.Name,
                IsActive = partner.IsActive
            });
        }

        [HttpGet("{id}/opponents")]
        public ActionResult<IEnumerable<UserResponse>> GetOpponents(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var opponents = _matchService.GetOpponents(id, userId);

            if (opponents == null || !opponents.Any())
                return NotFound(new { message = "Adversaires non trouvés" });

            var opponentResponses = opponents.Select(o => new UserResponse
            {
                Id = o.Id,
                Username = o.Username,
                Email = o.Email,
                FirstName = o.FirstName,
                LastName = o.LastName,
                SkillLevelId = o.SkillLevelId,
                SkillLevelName = o.SkillLevel?.Name,
                IsActive = o.IsActive
            });

            return Ok(opponentResponses);
        }

        [HttpGet("active")]
        public ActionResult<IEnumerable<UserResponse>> GetActiveUsers()
        {
            var users = _userService.GetActiveUsers();

            if (users == null)
                return NotFound();

            var responses = users.Select(u => new UserResponse
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                SkillLevelId = u.SkillLevelId,
                SkillLevelName = u.SkillLevel?.Name,
                IsActive = u.IsActive
            }).ToList();

            return Ok(responses);
        }

        [HttpPost]
        public ActionResult CreateMatch(CreateMatchRequest request)
        {
            try
            {
                var reservation = _reservationService.GetById(request.ReservationId);
                if (reservation == null)
                    return NotFound(new { message = "Réservation non trouvée" });

                // Vérifier que l'utilisateur a le droit de créer un match pour cette réservation
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var isAdmin = User.IsInRole("Admin");

                if (reservation.CreatedBy != userId && !isAdmin)
                    return Forbid();

                var match = new Match
                {
                    ReservationId = request.ReservationId
                };

                _matchService.Create(match, request.PlayerIds);
                return CreatedAtAction(nameof(GetMatchById), new { id = match.Id }, MapToResponse(match));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/players/{userId}")]
        public ActionResult AddPlayer(int id, int userId, [FromBody] AddPlayerRequest request)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var isAdmin = User.IsInRole("Admin");

                // Vérifier que l'utilisateur est admin ou ajoute lui-même
                if (userId != currentUserId && !isAdmin)
                    return Forbid();

                _matchService.AddPlayer(id, userId, request.Team);
                return Ok(new { message = "Joueur ajouté avec succès" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}/players/{userId}")]
        public ActionResult RemovePlayer(int id, int userId)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var isAdmin = User.IsInRole("Admin");

                // Vérifier que l'utilisateur est admin ou retire lui-même
                if (userId != currentUserId && !isAdmin)
                    return Forbid();

                _matchService.RemovePlayer(id, userId);
                return Ok(new { message = "Joueur retiré avec succès" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/status")]
        public ActionResult ChangeMatchStatus(int id, UpdateMatchStatusRequest request)
        {
            try
            {
                _matchService.ChangeStatus(id, request.Status);
                return Ok(new { message = "Statut du match mis à jour avec succès" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/complete")]
        public ActionResult CompleteMatch(int id, CompleteMatchRequest request)
        {
            try
            {
                _matchService.CompleteMatch(id, request.WinningTeamUserIds);
                return Ok(new { message = "Match terminé avec succès" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private MatchResponse MapToResponse(Match match)
        {
            // Récupérer les détails de la réservation
            var reservation = _reservationService.GetById(match.ReservationId);

            // Obtenir les informations sur les joueurs
            var matchPlayers = match.MatchPlayers ?? new List<MatchPlayer>();
            var playerResponses = matchPlayers.Select(mp => new MatchPlayerResponse
            {
                UserId = mp.UserId,
                Username = mp.User?.Username,
                Team = mp.Team,
                IsConfirmed = mp.IsConfirmed
            }).ToList();

            return new MatchResponse
            {
                Id = match.Id,
                ReservationId = match.ReservationId,
                StartDateTime = reservation?.StartDateTime ?? DateTime.MinValue,
                EndDateTime = reservation?.EndDateTime ?? DateTime.MinValue,
                CourtName = reservation?.Court?.Name,
                CreatedAt = match.CreatedAt,
                Status = match.Status,
                Players = playerResponses
            };
        }
    }
}
