using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelMatch.Models.Responses;

namespace PadelMatch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CourtsController : ControllerBase
    {
        private readonly ICourtService _courtService;

        public CourtsController(ICourtService courtService)
        {
            _courtService = courtService;
        }

        [HttpGet]        
        public ActionResult<ApiResponse<IEnumerable<CourtResponse>>> GetAllCourts()
        {
            try
            {
                var courts = _courtService.GetAllCourts();
                var courtResponses = courts.Select(MapToResponse);

                return Ok(new ApiResponse<IEnumerable<CourtResponse>>
                {
                    Success = true,
                    Message = "Liste des terrains récupérée avec succès",
                    Data = courtResponses
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<CourtResponse>>
                {
                    Success = false,
                    Message = $"Une erreur est survenue : {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ApiResponse<CourtResponse>> GetCourt(int id)
        {
            try
            {
                var court = _courtService.GetCourtById(id);
                if (court == null)
                    return NotFound(new ApiResponse<CourtResponse>
                    {
                        Success = false,
                        Message = $"Terrain avec l'ID {id} non trouvé"
                    });

                return Ok(new ApiResponse<CourtResponse>
                {
                    Success = true,
                    Message = "Terrain récupéré avec succès",
                    Data = MapToResponse(court)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<CourtResponse>
                {
                    Success = false,
                    Message = $"Une erreur est survenue : {ex.Message}"
                });
            }
        }

        [HttpGet("available")]        
        public ActionResult<ApiResponse<IEnumerable<CourtResponse>>> GetAvailableCourts(
            [FromQuery] DateTime date,
            [FromQuery] TimeSpan startTime,
            [FromQuery] TimeSpan endTime)
        {
            try
            {
                var courts = _courtService.GetAvailableCourts(date, startTime, endTime);
                var courtResponses = courts.Select(MapToResponse);

                return Ok(new ApiResponse<IEnumerable<CourtResponse>>
                {
                    Success = true,
                    Message = "Liste des terrains disponibles récupérée avec succès",
                    Data = courtResponses
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<CourtResponse>>
                {
                    Success = false,
                    Message = $"Une erreur est survenue : {ex.Message}"
                });
            }
        }

        [HttpGet("availability")]        
        public ActionResult<ApiResponse<IEnumerable<CourtAvailabilityResponse>>> GetCourtsAvailability(
            [FromQuery] DateTime date)
        {
            try
            {
                var courtsAvailability = _courtService.GetCourtsAvailabilityForDate(date);

                var response = courtsAvailability.Select(ca => new CourtAvailabilityResponse
                {
                    CourtId = ca.Court.Id,
                    CourtName = ca.Court.Name,
                    AvailableTimeSlots = ca.AvailableTimeSlots.Select(ts => new ReservationSlotResponse
                    {
                        StartTime = ts.StartTime,
                        EndTime = ts.EndTime,
                        Status = MapDomainStatusToApiStatus(ts.Status)
                    }).ToList()
                }).ToList();

                return Ok(new ApiResponse<IEnumerable<CourtAvailabilityResponse>>
                {
                    Success = true,
                    Message = $"Disponibilités des terrains pour le {date:dd/MM/yyyy} récupérées avec succès",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<CourtAvailabilityResponse>>
                {
                    Success = false,
                    Message = $"Une erreur est survenue lors de la récupération des disponibilités: {ex.Message}"
                });
            }
        }

        private CourtResponse MapToResponse(Court court)
        {
            return new CourtResponse
            {
                Id = court.Id,
                Name = court.Name,
                IsIndoor = court.IsIndoor,
                IsActive = court.IsActive
            };
        }

        private Domain.Enums.TimeSlotStatus MapDomainStatusToApiStatus(Domain.Enums.SlotAvailabilityStatus status)
        {
            return status switch
            {
                Domain.Enums.SlotAvailabilityStatus.Available => Domain.Enums.TimeSlotStatus.Available,
                Domain.Enums.SlotAvailabilityStatus.Booked => Domain.Enums.TimeSlotStatus.Booked,
                Domain.Enums.SlotAvailabilityStatus.Unavailable => Domain.Enums.TimeSlotStatus.Unavailable,
                _ => Domain.Enums.TimeSlotStatus.Unavailable
            };
        }
    }
}