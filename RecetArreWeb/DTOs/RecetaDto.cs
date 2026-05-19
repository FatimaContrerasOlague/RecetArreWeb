using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RecetArreWeb.DTOs
{
    public class RecetaDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("titulo")]
        public string Titulo { get; set; } = default!;
        
        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }
        
        [JsonPropertyName("instrucciones")]
        public string Instrucciones { get; set; } = default!;
        
        [JsonPropertyName("tiempoPreparacionMinutos")]
        public int TiempoPreparacionMinutos { get; set; }
        
        [JsonPropertyName("tiempoCoccionMinutos")]
        public int TiempoCoccionMinutos { get; set; }
        
        [JsonPropertyName("porciones")]
        public int Porciones { get; set; }
        
        [JsonPropertyName("estaPublicado")]
        public bool EstaPublicado { get; set; }
        
        [JsonPropertyName("creadoUtc")]
        public DateTime CreadoUtc { get; set; }
        
        [JsonPropertyName("modificadoUtc")]
        public DateTime ModificadoUtc { get; set; }
    }

    public class RecetaCreacionDto
    {
        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Titulo { get; set; } = default!;

        [StringLength(1000)]
        public string? Descripcion { get; set; }

        [Required]
        [StringLength(15000)]
        public string Instrucciones { get; set; } = default!;

        [Range(0, 24 * 60)]
        public int TiempoPreparacionMinutos { get; set; }

        [Range(0, 24 * 60)]
        public int TiempoCoccionMinutos { get; set; }

        [Range(1, 100)]
        public int Porciones { get; set; } = 1;

        public bool EstaPublicado { get; set; } = true;
    }

    public class RecetaModificacionDto
    {
        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Titulo { get; set; } = default!;

        [StringLength(1000)]
        public string? Descripcion { get; set; }

        [Required]
        [StringLength(15000)]
        public string Instrucciones { get; set; } = default!;

        [Range(0, 24 * 60)]
        public int TiempoPreparacionMinutos { get; set; }

        [Range(0, 24 * 60)]
        public int TiempoCoccionMinutos { get; set; }

        [Range(1, 100)]
        public int Porciones { get; set; } = 1;

        public bool EstaPublicado { get; set; } = true;
    }
}
