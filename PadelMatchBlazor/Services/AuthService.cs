using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PadelMatchBlazor.Auth;
using PadelMatchBlazor.Models;
using System.Net.Http.Json;

namespace PadelMatchBlazor.Services
{
    // Services/AuthService.cs
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient,
                          AuthenticationStateProvider authStateProvider,
                          ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<bool> Login(LoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);

            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            await _localStorage.SetItemAsync("authToken", result.Token);

            ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);

            return true;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
        }

        public async Task<bool> Register(RegisterRequest registerRequest)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerRequest);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                // Si vous voulez gérer les erreurs de validation côté serveur
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    // Vous pouvez lire les erreurs de validation si nécessaire
                    // var errors = await response.Content.ReadFromJsonAsync<Dictionary<string, string[]>>();
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'inscription: {ex.Message}");
                throw;
            }
        }
    }
}
