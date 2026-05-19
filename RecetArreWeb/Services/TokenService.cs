using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.JSInterop;

namespace RecetArreWeb.Services
{
    public interface ITokenService
    {
        Task GuardarToken(string token, DateTime expiracion);
        Task GuardarUsuarioId(string usuarioId);
        Task<string?> ObtenerToken();
        Task<string?> ObtenerUsuarioId();
        Task<DateTime?> ObtenerExpiracion();
        Task<bool> EstaAutenticado();
        Task EliminarToken();

    }
    public class TokenService : ITokenService
    {
        private readonly IJSRuntime jSRuntime;
        private const string TOKEN_KEY = "authToken";
        private const string EXPIRACION_KEY = "tokenExpiracion";
        private const string USUARIO_ID_KEY = "usuarioId";

        public TokenService(IJSRuntime jSRuntime)
        {
            this.jSRuntime = jSRuntime;
        }
        public async Task EliminarToken()
        {
            await jSRuntime.InvokeVoidAsync("localStorage.removeItem", TOKEN_KEY);
            await jSRuntime.InvokeVoidAsync("localStorage.removeItem", EXPIRACION_KEY);
            await jSRuntime.InvokeVoidAsync("localStorage.removeItem", USUARIO_ID_KEY);

        }

        public async Task<bool> EstaAutenticado()
        {
            var token = await ObtenerToken();
            return !string.IsNullOrEmpty(token);
            
        }

        public async Task GuardarToken(string token, DateTime expiracion)
        {
            await jSRuntime.InvokeVoidAsync("localStorage.setItem",TOKEN_KEY,token);
            await jSRuntime.InvokeVoidAsync("localStorage.setItem",EXPIRACION_KEY,expiracion.ToString("o"));
            //Formato ISO 8601 (2024-12-15T10:30:00Z)
            
        }

        public async Task GuardarUsuarioId(string usuarioId)
        {
            if (string.IsNullOrWhiteSpace(usuarioId))
            {
                Console.WriteLine("UsuarioId vacío; no se guardará en localStorage.");
                return;
            }

            await jSRuntime.InvokeVoidAsync("localStorage.setItem", USUARIO_ID_KEY, usuarioId);
        }

        public async Task<DateTime?> ObtenerExpiracion()
        {
            try
            {
                var expiracionStr = await jSRuntime.InvokeAsync<string?>("localStorage.getItem",EXPIRACION_KEY);
                if(string.IsNullOrEmpty(expiracionStr))
                    return null;
                if(DateTime.TryParse(expiracionStr, out var expiracion))
                    return expiracion;

                return null;
            }
            catch
            {
                return null;
            }
        }

        // Task<string> ITokenService.ObtenerToken()
        // {
        //     throw new NotImplementedException();
        // }

        public async Task<string?> ObtenerToken()
        {
            try
            {
                //1. Leer el token de localStorage
                var token = await jSRuntime.InvokeAsync<string?>("localStorage.getItem", TOKEN_KEY);

                //2. Si no hay token, devolver null
                if(string.IsNullOrEmpty(token))
                    return null;

                // Verificar si el token expiro
                var expiracion = await ObtenerExpiracion();
                if(expiracion.HasValue && expiracion.Value < DateTime.UtcNow)
                {
                    //Token expirado, eliminarlo
                    await EliminarToken();
                    return null;
                }
                return token;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el token: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> ObtenerUsuarioId()
        {
            try
            {
                var usuarioId = await jSRuntime.InvokeAsync<string?>("localStorage.getItem", USUARIO_ID_KEY);
                if (!string.IsNullOrWhiteSpace(usuarioId))
                {
                    return usuarioId;
                }

                var token = await ObtenerToken();
                if (string.IsNullOrWhiteSpace(token))
                {
                    return null;
                }

                var extraido = ExtraerUsuarioIdDelToken(token);
                if (!string.IsNullOrWhiteSpace(extraido))
                {
                    await GuardarUsuarioId(extraido);
                }

                return extraido;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el usuarioId: {ex.Message}");
                return null;
            }
        }

        private static string? ExtraerUsuarioIdDelToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var claims = jwtToken.Claims;

                return claims.FirstOrDefault(c =>
                    c.Type == ClaimTypes.NameIdentifier ||
                    c.Type == "sub" ||
                    c.Type == "userId" ||
                    c.Type == "id" ||
                    c.Type == "uid" ||
                    c.Type == "nameid")?.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al extraer usuarioId del token: {ex.Message}");
                return null;
            }
        }
    }
}
