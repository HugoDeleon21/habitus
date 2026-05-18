namespace Habitus.Api.DTOs
{
    public class HabitoUpdateRequest
    {
        public string Nome { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public DateOnly Data { get; set; }
    }
}
