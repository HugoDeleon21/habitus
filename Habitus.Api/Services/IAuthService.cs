using Habitus.Api.DTOs;

namespace Habitus.Api.Services
{
    public interface IAuthService
    {
        Task<AuthServiceResult<LoginResponse>> RegisterAsync(RegisterRequest request);

        Task<AuthServiceResult<LoginResponse>> LoginAsync(LoginRequest request);
    }
}
