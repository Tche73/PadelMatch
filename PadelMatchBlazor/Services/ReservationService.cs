 using PadelMatchBlazor.Models.Enums;
using PadelMatchBlazor.Models.Requests;
using PadelMatchBlazor.Models.Responses;
using System.Net.Http.Json;
using System.Text.Json;

namespace PadelMatchBlazor.Services
{
    public class ReservationService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ReservationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<ReservationResponse>> GetMyReservationsAsync()
        {
            try
            {
                // Modifiez cette ligne pour s'adapter au format renvoyé par l'API
                var response = await _httpClient.GetFromJsonAsync<List<ReservationResponse>>("api/reservations");
                return response ?? new List<ReservationResponse>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des réservations: {ex.Message}");
                return new List<ReservationResponse>();
            }
        }


        public async Task<ReservationResponse> GetReservationByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ReservationResponse>($"api/reservations/{id}");
                return response ?? new ReservationResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération de la réservation {id}: {ex.Message}");
                return new ReservationResponse();
            }
        }


        //public async Task<ReservationResponse> GetReservationByIdAsync(int id)
        //{
        //    try
        //    {
        //        var response = await _httpClient.GetFromJsonAsync<ApiResponse<ReservationResponse>>($"api/reservations/{id}", _jsonOptions);
        //        return response?.Data ?? new ReservationResponse();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Erreur lors de la récupération de la réservation {id}: {ex.Message}");
        //        return new ReservationResponse();
        //    }
        //}

        public async Task<bool> CreateReservationAsync(ReservationRequest reservation)
        {
            try
            {
                var createReservationRequest = new CreateReservationRequest
                {
                    CourtId = reservation.CourtId,
                    StartDateTime = reservation.StartTime,
                    EndDateTime = reservation.EndTime,
                    //Notes = reservation.Notes
                };

                var response = await _httpClient.PostAsJsonAsync("api/reservations", createReservationRequest);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la création de la réservation: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateReservationAsync(int id, ReservationRequest reservation)
        {
            try
            {
                var updateRequest = new CreateReservationRequest
                {
                    CourtId = reservation.CourtId,
                    StartDateTime = reservation.StartTime,
                    EndDateTime = reservation.EndTime
                };

                var response = await _httpClient.PutAsJsonAsync($"api/reservations/{id}", updateRequest);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la mise à jour de la réservation: {ex.Message}");
                return false;
            }
        }

        //public async Task<bool> UpdateReservationAsync(int id, ReservationRequest reservation)
        //{
        //    try
        //    {
        //        var response = await _httpClient.PutAsJsonAsync($"api/reservations/{id}", reservation);
        //        return response.IsSuccessStatusCode;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Erreur lors de la mise à jour de la réservation: {ex.Message}");
        //        return false;
        //    }
        //}

        //public async Task<bool> UpdateReservationAsync(int id, ReservationRequest reservation)
        //{
        //    try
        //    {
        //        var updateReservationRequest = new ReservationRequest
        //        {
        //            CourtId = reservation.CourtId,
        //            StartTime = reservation.StartTime,
        //            EndTime = reservation.EndTime,
        //            //Notes = reservation.Notes
        //        };

        //        var response = await _httpClient.PutAsJsonAsync($"api/reservations/{id}", updateReservationRequest);
        //        return response.IsSuccessStatusCode;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Erreur lors de la mise à jour de la réservation: {ex.Message}");
        //        return false;
        //    }
        //}

        public async Task<bool> CancelReservationAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/reservations/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'annulation de la réservation: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateReservationStatusAsync(int id, ReservationStatus status)
        {
            try
            {
                var request = new UpdateReservationStatusRequest
                {
                    Status = status
                };

                var response = await _httpClient.PutAsJsonAsync($"api/reservations/{id}/status", request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la mise à jour du statut: {ex.Message}");
                return false;
            }
        }
    }
}