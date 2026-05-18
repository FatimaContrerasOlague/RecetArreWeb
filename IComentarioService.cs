csharp RecetArreWeb/Services/ComentarioService.cs
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
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
        private readonly ITokenService? tokenService;
        private const string endpoint = "api/Comentarios";

        public ComentarioService(HttpClient httpClient, ITokenService? tokenService = null)
        {
            this.httpClient = httpClient;
            this.tokenService = tokenService;
        }

        public async Task<List<ComentarioDto>> ObtenerTodos()
        {
            try
            {
                var comentarios = await httpClient.GetFromJsonAsync<List<ComentarioDto>>(endpoint);
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
                var comentarios = await httpClient.GetFromJsonAsync<List<ComentarioDto>>($"{endpoint}/receta/{recetaId}");
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
                // Preparar request explícito para adjuntar header Authorization si hay token
                var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
                {
                    Content = JsonContent.Create(comentarioDto)
                };

                if (tokenService != null)
                {
                    var token = await tokenService.ObtenerToken();
                    Console.WriteLine($"[DEBUG] Token presente antes de POST: {(string.IsNullOrEmpty(token) ? "NO" : "SÍ")}");
                    if (!string.IsNullOrEmpty(token))
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                }

                var response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"POST {endpoint} fallo: {(int)response.StatusCode} - {response.ReasonPhrase}");
                    Console.WriteLine($"Body respuesta: {error}");
                    return null;
                }

                var creado = await response.Content.ReadFromJsonAsync<ComentarioDto>();
                Console.WriteLine($"POST {endpoint} éxito: Id={creado?.Id}");
                return creado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear comentario: {ex.Message}");
                return null;
            }
        }
    }
}