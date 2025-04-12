using PadelMatchBlazor.Models;
using PadelMatchBlazor.Models.Requests;
using PadelMatchBlazor.Models.Responses;
using System.Net.Http.Json;
using System.Text;

namespace PadelMatchBlazor.Services
{
    public class HttpUserService
    {
        private readonly HttpClient _httpClient;

        public HttpUserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<UserResponse>> SearchPlayersAsync(PlayerSearchRequest request)
        {
            var queryString = new StringBuilder("api/users/search?");

            if (request.CurrentUserId.HasValue)
                queryString.Append($"CurrentUserId={request.CurrentUserId}&");
            if (request.SkillLevelId.HasValue)
                queryString.Append($"SkillLevelId={request.SkillLevelId}&");
            if (request.SkillLevelTolerance.HasValue)
                queryString.Append($"SkillLevelTolerance={request.SkillLevelTolerance}&");
            if (request.DayOfWeek.HasValue)
                queryString.Append($"DayOfWeek={request.DayOfWeek}&");
            if (request.StartTime.HasValue)
                queryString.Append($"StartTime={request.StartTime.Value.ToString("hh\\:mm")}&");
            if (request.EndTime.HasValue)
                queryString.Append($"EndTime={request.EndTime.Value.ToString("hh\\:mm")}&");

            return await _httpClient.GetFromJsonAsync<IEnumerable<UserResponse>>(
                queryString.ToString().TrimEnd('&'));
        }

        public async Task<IEnumerable<SkillLevel>> GetSkillLevelsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<SkillLevel>>("api/skilllevels");
        }

        public async Task<UserResponse> GetUserByIdAsync(int userId)
        {
            return await _httpClient.GetFromJsonAsync<UserResponse>($"api/users/{userId}");
        }
    }
}
