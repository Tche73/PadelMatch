using Domain.Enums;

namespace PadelMatchBlazor.Models.Requests
{
    public class UpdateMatchStatusRequest
    {
        public MatchStatus Status { get; set; }
    }
}
