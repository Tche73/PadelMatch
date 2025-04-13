using PadelMatchBlazor.Models.Requests;
using PadelMatchBlazor.Models.Responses;
using System.Net.Http.Json;
using System.Text.Json;

namespace PadelMatchBlazor.Services
{
    public class CourtService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public CourtService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<CourtResponse>> GetAllCourtsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<CourtResponse>>>("api/courts", _jsonOptions);
                return response?.Data ?? new List<CourtResponse>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des terrains: {ex.Message}");
                return new List<CourtResponse>();
            }
        }

        public async Task<CourtResponse> GetCourtByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<CourtResponse>>($"api/courts/{id}", _jsonOptions);
                return response?.Data ?? new CourtResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération du terrain {id}: {ex.Message}");
                return new CourtResponse();
            }
        }

        public async Task<List<CourtResponse>> GetAvailableCourtsAsync(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            try
            {
                var formattedDate = date.ToString("yyyy-MM-dd");
                var formattedStartTime = startTime.ToString(@"hh\:mm\:ss");
                var formattedEndTime = endTime.ToString(@"hh\:mm\:ss");

                var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<CourtResponse>>>(
                    $"api/courts/available?date={formattedDate}&startTime={formattedStartTime}&endTime={formattedEndTime}",
                    _jsonOptions
                );

                return response?.Data ?? new List<CourtResponse>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des terrains disponibles: {ex.Message}");
                return new List<CourtResponse>();
            }
        }

        public async Task<List<CourtAvailabilityResponse>> GetCourtsAvailabilityAsync(DateTime date)
        {
            try
            {
                var formattedDate = date.ToString("yyyy-MM-dd");
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<CourtAvailabilityResponse>>>(
                    $"api/courts/availability?date={formattedDate}",
                    _jsonOptions
                );

                return response?.Data ?? new List<CourtAvailabilityResponse>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des disponibilités: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> CreateReservationAsync(ReservationRequest reservation)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/reservations", reservation);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la création de la réservation: {ex.Message}");
                return false;
            }
        }
    }
}