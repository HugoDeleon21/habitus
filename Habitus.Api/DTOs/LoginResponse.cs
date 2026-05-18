namespace Habitus.Api.DTOs
{
    public class LoginResponse
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Mensagem { get; set; } = string.Empty;
    }
}
