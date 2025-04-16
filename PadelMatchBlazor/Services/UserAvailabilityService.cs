using PadelMatch.Models.Requests;
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
            var response = await _httpClient.GetAsync("api/availabilities");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AvailabilityResponse>>()
                       ?? new List<AvailabilityResponse>();
            }

            // Lever une exception avec plus de détails
            throw new HttpRequestException($"Erreur {response.StatusCode}: {await response.Content.ReadAsStringAsync()}");
        }

        public async Task CreateUserAvailabilityAsync(AvailabilityRequest request)
        {
            var createRequest = new CreateAvailabilityRequest
            {
                DayOfWeek = (int)request.DayOfWeek,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                IsRecurring = request.IsRecurring
            };
            var response = await _httpClient.PostAsJsonAsync("api/availabilities", createRequest);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erreur {response.StatusCode}: {errorContent}");
            }
        }

        public async Task UpdateUserAvailabilityAsync(int id, AvailabilityRequest request)
        {
            var updateRequest = new UpdateAvailabilityRequest
            {
                DayOfWeek = (int)request.DayOfWeek,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                IsRecurring = request.IsRecurring
            };
            var response = await _httpClient.PutAsJsonAsync($"api/availabilities/{id}", updateRequest);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erreur {response.StatusCode}: {errorContent}");
            }
        }

        // Nouvelle méthode pour supprimer une disponibilité
        public async Task DeleteUserAvailabilityAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/availabilities/{id}");
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erreur {response.StatusCode}: {errorContent}");
            }           
        }
    }
}