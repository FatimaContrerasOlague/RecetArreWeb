using System.Text.Json.Serialization;

namespace RecetArreWeb.DTOs
{
    public class CategoriaDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = default!;
        
        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }
        
        [JsonPropertyName("creadoUtc")]
        public DateTime CreadoUtc { get; set; }
    }

    public class CategoriaCreacionDto
    {   
        public string Nombre { get; set; } = default!;
        public string? Descripcion { get; set; }
    }

    public class CategoriaModificacionDto
    {
        public string Nombre { get; set; } = default!;
        public string? Descripcion { get; set; }
    }
}
