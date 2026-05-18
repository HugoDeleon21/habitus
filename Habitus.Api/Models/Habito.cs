namespace Habitus.Api.Models
{
    public class Habito
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public DateOnly Data { get; set; }

        public bool Concluido { get; set; } = false;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }
    }
}