using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using RecetArreWeb.Services;

// API (servidor) - habilitar CORS en desarrollo para permitir llamadas desde el cliente
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Registrar servicio de comentarios y, si usas autenticación, el token service
builder.Services.AddScoped<IComentarioService, ComentarioService>();
// builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevAllowClient", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin(); // usar AllowAnyOrigin sólo en dev; en prod especifica orígenes
    });
});

await builder.Build().RunAsync();