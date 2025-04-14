using PadelMatchBlazor.Models.Requests;
using PadelMatchBlazor.Models.Responses;
using System.Text;

namespace PadelMatchBlazor.Services
{
    // Services/UserService.cs
    public class UserService
    {
        private readonly HttpService _httpService;

        public UserService(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {
            return await _httpService.GetAsync<IEnumerable<UserResponse>>("api/users");
        }

        public async Task<IEnumerable<UserResponse>> SearchPlayersAsync(PlayerSearchRequest request)
        {
            // Construire l'URL avec les paramètres de recherche
            var queryString = new StringBuilder("api/users/search?");

            if (request.CurrentUserId.HasValue)
                queryString.Append($"CurrentUserId={request.CurrentUserId}&");

            // Ajoutez les autres paramètres
            // ...

            return await _httpService.GetAsync<IEnumerable<UserResponse>>(queryString.ToString().TrimEnd('&'));
        }

        public async Task<UserResponse> GetCurrentUserProfileAsync()
        {
            return await _httpService.GetAsync<UserResponse>("api/users/me");
        }

        // Autres méthodes...
    }
}
