using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelMatch.Models.Requests;
using PadelMatch.Models.Responses;
using System.Security.Claims;

namespace PadelMatch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AvailabilitiesController : ControllerBase
    {
        private readonly IAvailabilityService _availabilityService;

        public AvailabilitiesController(IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AvailabilityResponse>> GetUserAvailabilities()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var availabilities = _availabilityService.GetByUserId(userId);
            var responses = availabilities.Select(MapToResponse);
            return Ok(responses);
        }

        [HttpGet("{id}")]
        public ActionResult<AvailabilityResponse> GetAvailabilityById(int id)
        {
            var availability = _availabilityService.GetById(id);
            if (availability == null)
                return NotFound();

            // Vérifier que l'utilisateur a accès à cette disponibilité
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var isAdmin = User.IsInRole("Admin");

            if (availability.UserId != userId && !isAdmin)
                return Forbid();

            return Ok(MapToResponse(availability));
        }

        [HttpPost]
        public ActionResult CreateAvailability(CreateAvailabilityRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                var availability = new Availability
                {
                    UserId = userId,
                    DayOfWeek = request.DayOfWeek,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    IsRecurring = request.IsRecurring
                };

                _availabilityService.Create(availability);
                return CreatedAtAction(nameof(GetAvailabilityById), new { id = availability.Id }, MapToResponse(availability));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public ActionResult UpdateAvailability(int id, UpdateAvailabilityRequest request)
        {
            try
            {
                var availability = _availabilityService.GetById(id);
                if (availability == null)
                    return NotFound();

                // Vérifier que l'utilisateur a le droit de modifier cette disponibilité
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var isAdmin = User.IsInRole("Admin");

                if (availability.UserId != userId && !isAdmin)
                    return Forbid();

                availability.DayOfWeek = request.DayOfWeek;
                availability.StartTime = request.StartTime;
                availability.EndTime = request.EndTime;
                availability.IsRecurring = request.IsRecurring;

                _availabilityService.Update(availability);
                return Ok(new { message = "Disponibilité mise à jour avec succès" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteAvailability(int id)
        {
            try
            {
                var availability = _availabilityService.GetById(id);
                if (availability == null)
                    return NotFound();

                // Vérifier que l'utilisateur a le droit de supprimer cette disponibilité
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var isAdmin = User.IsInRole("Admin");

                if (availability.UserId != userId && !isAdmin)
                    return Forbid();

                _availabilityService.Delete(id);
                return Ok(new { message = "Disponibilité supprimée avec succès" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private AvailabilityResponse MapToResponse(Availability availability)
        {
            return new AvailabilityResponse
            {
                Id = availability.Id,
                UserId = availability.UserId,
                UserName = availability.User?.Username,
                DayOfWeek = availability.DayOfWeek,
                StartTime = availability.StartTime,
                EndTime = availability.EndTime,
                IsRecurring = availability.IsRecurring
            };
        }
    }
}