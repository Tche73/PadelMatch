using PadelMatchBlazor.Models;
using System.Net.Http.Json;

namespace PadelMatchBlazor.Services
{
    public class CourtService
    {
        private readonly HttpClient _httpClient;

        public CourtService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CourtResponse>> GetAllCourtsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CourtResponse>>("api/courts") ?? new List<CourtResponse>();
        }

        public async Task<List<CourtAvailabilityResponse>> GetCourtsAvailabilityAsync(DateTime date)
        {
            var formattedDate = date.ToString("yyyy-MM-dd");
            return await _httpClient.GetFromJsonAsync<List<CourtAvailabilityResponse>>($"api/courts/availability?date={formattedDate}") ?? new List<CourtAvailabilityResponse>();
        }

        public async Task<bool> CreateReservationAsync(ReservationRequest reservation)
        {
            var response = await _httpClient.PostAsJsonAsync("api/reservations", reservation);
            return response.IsSuccessStatusCode;
        }
    }

    public class ReservationRequest
    {
        public int CourtId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Notes { get; set; }
    }
}
