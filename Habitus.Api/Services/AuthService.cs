using Habitus.Api.Data;
using Habitus.Api.DTOs;
using Habitus.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Habitus.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AuthServiceResult<LoginResponse>> RegisterAsync(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Nome) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Senha))
            {
                return AuthServiceResult<LoginResponse>.Fail(
                    "Nome, email e senha são obrigatórios.",
                    AuthErrorType.Validation);
            }

            var email = request.Email.Trim().ToLowerInvariant();
            var emailJaExiste = await _context.Usuarios.AnyAsync(usuario => usuario.Email.ToLower() == email);

            if (emailJaExiste)
            {
                return AuthServiceResult<LoginResponse>.Fail(
                    "Já existe um usuário cadastrado com este email.",
                    AuthErrorType.Conflict);
            }

            var usuario = new Usuario
            {
                Nome = request.Nome.Trim(),
                Email = email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha)
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var response = new LoginResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Mensagem = "Cadastro realizado com sucesso."
            };

            return AuthServiceResult<LoginResponse>.Ok(response, response.Mensagem);
        }

        public async Task<AuthServiceResult<LoginResponse>> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha))
            {
                return AuthServiceResult<LoginResponse>.Fail(
                    "Email e senha são obrigatórios.",
                    AuthErrorType.Validation);
            }

            var email = request.Email.Trim().ToLowerInvariant();
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(usuario => usuario.Email.ToLower() == email);

            if (usuario is null || !BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash))
            {
                return AuthServiceResult<LoginResponse>.Fail(
                    "Email ou senha inválidos.",
                    AuthErrorType.Unauthorized);
            }

            var response = new LoginResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Mensagem = "Login realizado com sucesso."
            };

            return AuthServiceResult<LoginResponse>.Ok(response, response.Mensagem);
        }
    }
}
