using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PadelMatchBlazor;
using PadelMatchBlazor.Auth;
using PadelMatchBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


// Ajouter les services
builder.Services.AddScoped<HttpService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<SkillLevelService>();
builder.Services.AddScoped<CourtService>();
builder.Services.AddScoped<UserAvailabilityService>();
builder.Services.AddScoped<MatchClientService>();   
builder.Services.AddScoped(sp =>
{
    var localStorage = sp.GetRequiredService<ILocalStorageService>();
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    return new AuthorizationMessageHandler(localStorage, navigationManager);
});


// Configurez le HttpClient via IHttpClientFactory
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("https://localhost:7109/");
})
.AddHttpMessageHandler<AuthorizationMessageHandler>();

// Configurez l'injection du HttpClient dans vos services
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("API"));

builder.Services.AddScoped<HttpUserService>();
// Ajouter le gestionnaire d'�tat d'authentification personnalis�
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// Ajouter les services Blazor d'authentification
builder.Services.AddAuthorizationCore();

// Ajouter le service de stockage local
builder.Services.AddBlazoredLocalStorage();
await builder.Build().RunAsync();
