using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;
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
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ReservationResponse>> GetReservations()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var isAdmin = User.IsInRole("Admin");

            IEnumerable<Reservation> reservations;
            if (isAdmin)
            {
                reservations = _reservationService.GetAll();
            }
            else
            {
                reservations = _reservationService.GetByUserId(userId);
            }

            var reservationResponses = reservations.Select(MapToResponse);
            return Ok(reservationResponses);
        }

        [HttpGet("{id}")]
        public ActionResult<ReservationResponse> GetReservationById(int id)
        {
            var reservation = _reservationService.GetById(id);
            if (reservation == null)
                return NotFound();

            // Vérifier que l'utilisateur a accès à cette réservation
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var isAdmin = User.IsInRole("Admin");

            if (reservation.CreatedBy != userId && !isAdmin)
                return Forbid();

            return Ok(MapToResponse(reservation));
        }

        [HttpGet("court/{courtId}")]
        public ActionResult<IEnumerable<ReservationResponse>> GetReservationsByCourtId(int courtId)
        {
            var reservations = _reservationService.GetByCourtId(courtId);
            var reservationResponses = reservations.Select(MapToResponse);
            return Ok(reservationResponses);
        }

        [HttpGet("available")]
        public ActionResult<IEnumerable<ReservationResponse>> GetAvailableTimeSlots(int courtId, DateTime date)
        {
            var availableSlots = _reservationService.GetAvailableTimeSlots(courtId, date);
            var slotResponses = availableSlots.Select(MapToResponse);
            return Ok(slotResponses);
        }

        [HttpPost]
        public ActionResult CreateReservation(CreateReservationRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                var reservation = new Reservation
                {
                    CourtId = request.CourtId,
                    CreatedBy = userId,
                    StartDateTime = request.StartDateTime,
                    EndDateTime = request.EndDateTime,
                    Status = ReservationStatus.Pending
                };

                _reservationService.Create(reservation);
                return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, MapToResponse(reservation));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public ActionResult UpdateReservation(int id, CreateReservationRequest request)
        {
            try
            {
                var reservation = _reservationService.GetById(id);
                if (reservation == null)
                    return NotFound();

                // Vérifier que l'utilisateur a le droit de modifier cette réservation
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var isAdmin = User.IsInRole("Admin");

                if (reservation.CreatedBy != userId && !isAdmin)
                    return Forbid();

                reservation.CourtId = request.CourtId;
                reservation.StartDateTime = request.StartDateTime;
                reservation.EndDateTime = request.EndDateTime;

                _reservationService.Update(reservation);
                return Ok(new { message = "Réservation mise à jour avec succès" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/status")]
        public ActionResult ChangeReservationStatus(int id, UpdateReservationStatusRequest request)
        {
            try
            {
                var reservation = _reservationService.GetById(id);
                if (reservation == null)
                    return NotFound();

                // Vérifier que l'utilisateur a le droit de modifier cette réservation
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var isAdmin = User.IsInRole("Admin");

                if (reservation.CreatedBy != userId && !isAdmin)
                    return Forbid();

                _reservationService.ChangeStatus(id, request.Status);
                return Ok(new { message = "Statut de la réservation mis à jour avec succès" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public ActionResult CancelReservation(int id)
        {
            try
            {
                var reservation = _reservationService.GetById(id);
                if (reservation == null)
                    return NotFound();

                // Vérifier que l'utilisateur a le droit d'annuler cette réservation
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var isAdmin = User.IsInRole("Admin");

                if (reservation.CreatedBy != userId && !isAdmin)
                    return Forbid();

                _reservationService.Cancel(id);
                return Ok(new { message = "Réservation annulée avec succès" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private ReservationResponse MapToResponse(Reservation reservation)
        {
            return new ReservationResponse
            {
                Id = reservation.Id,
                CourtId = reservation.CourtId,
                CourtName = reservation.Court?.Name,
                CreatedBy = reservation.CreatedBy,
                CreatorName = reservation.Creator?.Username,
                StartDateTime = reservation.StartDateTime,
                EndDateTime = reservation.EndDateTime,
                CreatedAt = reservation.CreatedAt,
                Status = reservation.Status
            };
        }
    }
}
