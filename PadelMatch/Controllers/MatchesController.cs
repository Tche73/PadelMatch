using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using PadelMatch.Models.Requests;
using PadelMatch.Models.Responses;
using System.Security.Claims;

namespace PadelMatch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpPost]
        public ActionResult CreateMatch(CreateMatchRequest request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var match = new Match
                {
                    ReservationId = request.ReservationId,
                    CreatedBy = userId 
                };

                _matchService.Create(match, request.PlayerIds);
                return CreatedAtAction(nameof(GetMatchById), new { id = match.Id }, MapToResponse(match));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<MatchResponse>> GetMatches()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var matches = _matchService.GetByUserIdWithPlayers(userId);

            // Ajouter des logs
            Console.WriteLine($"Utilisateur {userId}: {matches.Count()} matches trouvés");

            foreach (var match in matches)
            {
                Console.WriteLine($"Match {match.Id}: {match.MatchPlayers?.Count ?? 0} joueurs");
            }

            var matchResponses = matches.Select(MapToResponse).ToList();

            // Vérifier les données de sortie
            Console.WriteLine($"Envoi de {matchResponses.Count} réponses");
            foreach (var resp in matchResponses)
            {
                Console.WriteLine($"Réponse {resp.Id}: {resp.Players?.Count ?? 0} joueurs");
            }

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

        [HttpGet("debug")]
        public ActionResult DebugMatches()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var matches = _matchService.GetByUserIdWithPlayers(userId);
            var details = matches.Select(m => new {
                MatchId = m.Id,
                PlayerCount = m.MatchPlayers?.Count ?? 0,
                Players = m.MatchPlayers?.Select(p => new { p.UserId, p.User?.FirstName, p.Team })
            });
            return Ok(details);
        }

        private MatchResponse MapToResponse(Match match)
        {
            var reservation = _reservationService.GetById(match.ReservationId);

            var playerResponses = match.MatchPlayers?.Select(mp => new MatchPlayerResponse
            {
                UserId = mp.UserId,
                Username = mp.User?.Username,
                FirstName = mp.User?.FirstName,
                LastName = mp.User?.LastName,
                Team = mp.Team,
                IsConfirmed = mp.IsConfirmed,
                IsOrganizer = mp.UserId == match.CreatedBy
            }).ToList() ?? new List<MatchPlayerResponse>();

            return new MatchResponse
            {
                Id = match.Id,
                ReservationId = match.ReservationId,
                StartDateTime = reservation?.StartDateTime ?? DateTime.MinValue,
                EndDateTime = reservation?.EndDateTime ?? DateTime.MinValue,
                CourtName = reservation?.Court?.Name,
                CreatedAt = match.CreatedAt,
                Status = match.Status,
                Players = playerResponses,
                Team1Players = playerResponses.Where(p => p.Team == 1).ToList(),
                Team2Players = playerResponses.Where(p => p.Team == 2).ToList(),
                CreatedBy = match.CreatedBy,
                CreatorUsername = match.Creator?.Username
            };
        }
    }
}
