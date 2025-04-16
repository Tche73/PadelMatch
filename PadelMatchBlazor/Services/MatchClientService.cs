using Domain.Enums;
using PadelMatchBlazor.Models.Requests;
using PadelMatchBlazor.Models.Responses;
using System.Net.Http.Json;

namespace PadelMatchBlazor.Services
{
    public class MatchClientService
    {
        private readonly HttpClient _httpClient;

        public MatchClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<MatchResponse>> GetMatchesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/matches");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<MatchResponse>>() ?? new List<MatchResponse>();
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Erreur lors de la récupération des matches: {ex.Message}");
            }
        }

        public async Task<MatchResponse> GetMatchByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/matches/{id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<MatchResponse>();
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Erreur lors de la récupération du match: {ex.Message}");
            }
        }

        public async Task<UserResponse> GetPartnerAsync(int matchId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/matches/{matchId}/partner");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<UserResponse>();
            }
            catch
            {
                return null; // Gérer le cas où le partenaire n'est pas trouvé
            }
        }

        public async Task<List<UserResponse>> GetOpponentsAsync(int matchId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/matches/{matchId}/opponents");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<UserResponse>>() ?? new List<UserResponse>();
            }
            catch
            {
                return new List<UserResponse>(); // Retourner une liste vide en cas d'erreur
            }
        }

        public async Task CreateMatchAsync(CreateMatchRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/matches", request);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erreur lors de la création du match: {error}");
            }
        }

        public async Task AddPlayerAsync(int matchId, int userId, int team)
        {
            var request = new AddPlayerRequest { Team = team };
            var response = await _httpClient.PutAsJsonAsync($"api/matches/{matchId}/players/{userId}", request);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erreur lors de l'ajout du joueur: {error}");
            }
        }

        public async Task RemovePlayerAsync(int matchId, int userId)
        {
            var response = await _httpClient.DeleteAsync($"api/matches/{matchId}/players/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erreur lors du retrait du joueur: {error}");
            }
        }

        public async Task ChangeMatchStatusAsync(int matchId, MatchStatus status)
        {
            var request = new UpdateMatchStatusRequest { Status = status };
            var response = await _httpClient.PutAsJsonAsync($"api/matches/{matchId}/status", request);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erreur lors du changement de statut: {error}");
            }
        }

        public async Task CompleteMatchAsync(int matchId, List<int> winningTeamUserIds)
        {
            var request = new CompleteMatchRequest { WinningTeamUserIds = winningTeamUserIds };
            var response = await _httpClient.PutAsJsonAsync($"api/matches/{matchId}/complete", request);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erreur lors de la complétion du match: {error}");
            }
        }
    }
}