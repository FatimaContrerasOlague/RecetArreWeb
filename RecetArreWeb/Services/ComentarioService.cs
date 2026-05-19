using System.Net.Http.Json;
using System.Text.Json;
using RecetArreWeb.DTOs;

namespace RecetArreWeb.Services
{
    public interface IComentarioService
    {
        Task<List<ComentarioDto>> ObtenerTodos();
        Task<List<ComentarioDto>> ObtenerPorReceta(int recetaId);
        Task<ComentarioDto?> CrearComentario(ComentarioCreacionDto comentarioDto);
    }

    public class ComentarioService : IComentarioService
    {
        private readonly HttpClient httpClient;
        private const string endpoint = "api/Comentarios";

        public ComentarioService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<ComentarioDto>> ObtenerTodos()
        {
            try
            {
                var response = await httpClient.GetAsync(endpoint);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error al obtener comentarios: {response.StatusCode}");
                    return new List<ComentarioDto>();
                }

                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"JSON Response: {json}");
                var comentarios = JsonSerializer.Deserialize<List<ComentarioDto>>(json, JsonOptions.Default);
                return comentarios ?? new List<ComentarioDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener comentarios: {ex.Message}");
                return new List<ComentarioDto>();
            }
        }

        public async Task<List<ComentarioDto>> ObtenerPorReceta(int recetaId)
        {
            try
            {
                var response = await httpClient.GetAsync($"{endpoint}/receta/{recetaId}");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error al obtener comentarios de la receta {recetaId}: {response.StatusCode}");
                    return new List<ComentarioDto>();
                }

                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"JSON Response: {json}");
                var comentarios = JsonSerializer.Deserialize<List<ComentarioDto>>(json, JsonOptions.Default);
                return comentarios ?? new List<ComentarioDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener comentarios de la receta {recetaId}: {ex.Message}");
                return new List<ComentarioDto>();
            }
        }

        public async Task<ComentarioDto?> CrearComentario(ComentarioCreacionDto comentarioDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync(endpoint, comentarioDto, JsonOptions.Default);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al crear comentario (Status: {response.StatusCode}): {error}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"JSON Response: {json}");
                var result = JsonSerializer.Deserialize<ComentarioDto>(json, JsonOptions.Default);
                if (result != null)
                {
                    Console.WriteLine($"Comentario creado exitosamente: {result.Id}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear comentario: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }
    }
}
