// Ejemplo mínimo (API server): habilitar CORS aquí, no en el cliente.
var builder = WebApplication.CreateBuilder(args);

// ... registros habituales (DbContext, Identity, AutoMapper, Controllers)
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevAllowClient", policy =>
    {
        policy.WithOrigins("http://localhost:5000", "https://localhost:5001") // ajusta orígenes
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors("DevAllowClient");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();