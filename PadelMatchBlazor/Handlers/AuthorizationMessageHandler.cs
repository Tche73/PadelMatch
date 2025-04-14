using Azure;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Headers;

public class AuthorizationMessageHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navigationManager;

    public AuthorizationMessageHandler(ILocalStorageService localStorage, NavigationManager navigationManager)
    {
        _localStorage = localStorage;
        _navigationManager = navigationManager;
    }

    //protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    //{
    //    var token = await _localStorage.GetItemAsync<string>("authToken");

    //    if (!string.IsNullOrEmpty(token))
    //    {
    //        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
    //    }

    //    // Envoyer la requête
    //    var response = await base.SendAsync(request, cancellationToken);

    //    // Vérifier si la réponse est 401 (Unauthorized)
    //    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
    //    {
    //        // Supprimer le token périmé
    //        await _localStorage.RemoveItemAsync("authToken");

    //        // Rediriger vers la page de connexion
    //        _navigationManager.NavigateTo("/login?expired=true", forceLoad: true);
    //    }

    //    return await base.SendAsync(request, cancellationToken);
    //}
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Envoyer la requête
        var response = await base.SendAsync(request, cancellationToken);

        // Vérifier si la réponse est 401 (Unauthorized)
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            // Supprimer le token périmé
            await _localStorage.RemoveItemAsync("authToken");

            // Rediriger vers la page de connexion
            _navigationManager.NavigateTo("/login?expired=true", forceLoad: true);
        }

        return response;
    }
}