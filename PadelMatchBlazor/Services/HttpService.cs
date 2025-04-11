using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;

namespace PadelMatchBlazor.Services
{
        public class HttpService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;

        public HttpService(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
        }

        public async Task<T> GetAsync<T>(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return await SendRequestAsync<T>(request);
        }

        // Ajoutez les méthodes Post, Put, Delete
        // ...

        private async Task<T> SendRequestAsync<T>(HttpRequestMessage request)
        {
            // Ajoutez l'authentification JWT ici
            // ...

            var response = await _httpClient.SendAsync(request);

            // Gérer les erreurs HTTP
            // ...

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
