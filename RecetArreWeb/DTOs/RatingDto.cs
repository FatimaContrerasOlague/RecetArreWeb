using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RecetArreWeb.DTOs
{
    public class RatingDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("estrellas")]
        public double Estrellas { get; set; }
        
        [JsonPropertyName("usuarioId")]
        public string UsuarioId { get; set; } = default!;
        
        [JsonPropertyName("recetaId")]
        public int RecetaId { get; set; }

        [JsonPropertyName("creadoUtc")]
        public DateTime CreadoUtc { get; set; }
    }

    public class RatingCreacionDto
    {
        [Range(1, 5)]
        public int Estrellas { get; set; }

        public string UsuarioId { get; set; } = string.Empty;

        [Required]
        public int RecetaId { get; set; }
    }

    public class RatingModificacionDto
    {
        [Range(1, 5)]
        public int Estrellas { get; set; }

        public string UsuarioId { get; set; } = string.Empty;

        [Required]
        public int RecetaId { get; set; }
    }
}
