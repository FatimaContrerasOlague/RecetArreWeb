using System.ComponentModel.DataAnnotations;

namespace RecetArreWeb.DTOs
{
    public class RatingDto
    {
        public int Id { get; set; }
        public int Estrellas { get; set; }
        public string UsuarioId { get; set; } = default!;
        public int RecetaId { get; set; }
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
