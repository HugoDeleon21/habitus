namespace Habitus.Api.DTOs
{
    public class HabitoResponse
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public DateOnly Data { get; set; }

        public bool Concluido { get; set; }

        public DateTime DataCriacao { get; set; }

        public int UsuarioId { get; set; }
    }
}
