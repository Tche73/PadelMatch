using Domain.Enums;

namespace PadelMatch.Models.Requests
{
    public class UpdateMatchStatusRequest
    {
        public MatchStatus Status { get; set; }
    }
}
