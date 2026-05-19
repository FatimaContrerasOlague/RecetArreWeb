using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RecetArreWeb.DTOs
{
    public class ComentarioDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("contenido")]
        public string Contenido { get; set; } = default!;
        
        [JsonPropertyName("creadoUtc")]
        public DateTime CreadoUtc { get; set; }
        
        [JsonPropertyName("recetaId")]
        public int RecetaId { get; set; }
        
        [JsonPropertyName("usuarioId")]
        public string UsuarioId { get; set; } = default!;
        
        [JsonPropertyName("usuarioNombre")]
        public string UsuarioNombre { get; set; } = default!;
    }

    public class ComentarioCreacionDto
    {
        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Contenido { get; set; } = default!;

        [Required]
        public int RecetaId { get; set; }
    }
}
