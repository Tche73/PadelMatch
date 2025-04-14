using PadelMatchBlazor.Models.Requests;
using PadelMatchBlazor.Models.Responses;
using System.Net.Http.Json;

namespace PadelMatchBlazor.Services
{
    public class UserAvailabilityService
    {
        private readonly HttpClient _httpClient;

        public UserAvailabilityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AvailabilityResponse>> GetUserAvailabilitiesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<AvailabilityResponse>>("api/availabilities");
        }

        public async Task CreateUserAvailabilityAsync(AvailabilityRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/availabilities", request);
            response.EnsureSuccessStatusCode();
        }

        // Nouvelle méthode pour supprimer une disponibilité
        public async Task DeleteUserAvailabilityAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/availabilities/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}