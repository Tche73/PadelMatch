using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PadelMatch.Models.Responses;

namespace PadelMatch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourtsController : ControllerBase
    {
        private readonly ICourtService _courtService;

        public CourtsController(ICourtService courtService)
        {
            _courtService = courtService;
        }

        [HttpGet("available")]
        [Authorize]
        public ActionResult<IEnumerable<CourtResponse>> GetAvailableCourts([FromQuery] DateTime date, [FromQuery] TimeSpan startTime, [FromQuery] TimeSpan endTime)
        {
            var courts = _courtService.GetAvailableCourts(date, startTime, endTime);
            var courtResponses = courts.Select(MapToResponse);
            return Ok(courtResponses);
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
    }
}
