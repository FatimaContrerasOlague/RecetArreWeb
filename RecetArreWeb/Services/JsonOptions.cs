using System.Text.Json;
using System.Text.Json.Serialization;

namespace RecetArreWeb.Services
{
    /// <summary>
    /// Opciones de serialización JSON centralizadas para toda la aplicación.
    /// Configura case-insensitive matching y usa camelCase para el nombrado de propiedades.
    /// </summary>
    public static class JsonOptions
    {
        public static readonly JsonSerializerOptions Default = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = false,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }
}
