using Blazored.SessionStorage;
using GestorMat.Frontend;
using GestorMat.Frontend.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient hacia la API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7028/")
});
builder.Services.AddScoped<AuthService>();

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<AuthHttpClient>();
builder.Services.AddScoped<NavigationService>();
builder.Services.AddScoped<MaterialService>();
builder.Services.AddScoped<DepositoService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

await builder.Build().RunAsync();
