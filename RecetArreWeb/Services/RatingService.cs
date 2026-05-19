using System.Net.Http.Json;
using System.Text.Json;
using RecetArreWeb.DTOs;

namespace RecetArreWeb.Services
{
    public interface IRatingService
    {
        Task<List<RatingDto>> ObtenerTodos();
        Task<List<RatingDto>> ObtenerPorReceta(int recetaId);
        Task<RatingDto?> CrearRating(RatingCreacionDto ratingDto);
    }

    public class RatingService : IRatingService
    {
        private readonly HttpClient httpClient;
        private const string endpoint = "api/Ratings";

        public RatingService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<RatingDto>> ObtenerTodos()
        {
            try
            {
                var response = await httpClient.GetAsync(endpoint);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error al obtener ratings: {response.StatusCode}");
                    return new List<RatingDto>();
                }

                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"JSON Response: {json}");
                var ratings = JsonSerializer.Deserialize<List<RatingDto>>(json, JsonOptions.Default);
                return ratings ?? new List<RatingDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener ratings: {ex.Message}");
                return new List<RatingDto>();
            }
        }

        public async Task<List<RatingDto>> ObtenerPorReceta(int recetaId)
        {
            try
            {
                var response = await httpClient.GetAsync($"{endpoint}/receta/{recetaId}");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error al obtener ratings de la receta {recetaId}: {response.StatusCode}");
                    return new List<RatingDto>();
                }

                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"JSON Response: {json}");
                var ratings = JsonSerializer.Deserialize<List<RatingDto>>(json, JsonOptions.Default);
                return ratings ?? new List<RatingDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener ratings de la receta {recetaId}: {ex.Message}");
                return new List<RatingDto>();
            }
        }

        public async Task<RatingDto?> CrearRating(RatingCreacionDto ratingDto)
        {
            try
            {
                var usuarioId = string.IsNullOrWhiteSpace(ratingDto.UsuarioId) ? null : ratingDto.UsuarioId;
                var payload = new
                {
                    recetaId = ratingDto.RecetaId,
                    estrellas = ratingDto.Estrellas,
                    usuarioId,
                    userId = usuarioId
                };

                var response = await httpClient.PostAsJsonAsync(endpoint, payload, JsonOptions.Default);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al crear rating (Status: {response.StatusCode}): {error}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"JSON Response: {json}");
                var result = JsonSerializer.Deserialize<RatingDto>(json, JsonOptions.Default);
                if (result != null)
                {
                    Console.WriteLine($"Rating creado exitosamente: {result.Id}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear rating: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }
    }
}
