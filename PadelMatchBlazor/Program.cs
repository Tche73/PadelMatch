//using Application.Services.Implementations;
using Application.Services.Implementations;
using Blazored.LocalStorage;
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
builder.Services.AddScoped<PadelMatchBlazor.Services.UserService>();
builder.Services.AddScoped<PadelMatchBlazor.Services.ReservationService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthorizationMessageHandler>();
builder.Services.AddScoped<SkillLevelService>();
builder.Services.AddScoped<PadelMatchBlazor.Services.CourtService>();


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
// Ajouter le gestionnaire d'état d'authentification personnalisé
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// Ajouter les services Blazor d'authentification
builder.Services.AddAuthorizationCore();

// Ajouter le service de stockage local
builder.Services.AddBlazoredLocalStorage();
await builder.Build().RunAsync();
