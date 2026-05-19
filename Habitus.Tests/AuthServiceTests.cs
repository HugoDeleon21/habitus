using Habitus.Api.Data;
using Habitus.Api.DTOs;
using Habitus.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Habitus.Tests;

public class AuthServiceTests
{
    [Fact]
    public async Task DeveCadastrarUsuarioComDadosValidos()
    {
        using var context = CriarContexto();
        var service = new AuthService(context);
        var request = new RegisterRequest
        {
            Nome = "Ana Silva",
            Email = "ana@habitus.test",
            Senha = "Senha123"
        };

        var result = await service.RegisterAsync(request);

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal("Ana Silva", result.Data.Nome);
        Assert.Equal("ana@habitus.test", result.Data.Email);
        Assert.Single(context.Usuarios);
    }

    [Fact]
    public async Task NaoDeveCadastrarUsuarioComEmailDuplicado()
    {
        using var context = CriarContexto();
        var service = new AuthService(context);
        var request = new RegisterRequest
        {
            Nome = "Ana Silva",
            Email = "ana@habitus.test",
            Senha = "Senha123"
        };

        await service.RegisterAsync(request);
        var result = await service.RegisterAsync(request);

        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.Equal(AuthErrorType.Conflict, result.ErrorType);
        Assert.Single(context.Usuarios);
    }

    [Fact]
    public async Task DeveRealizarLoginComCredenciaisValidas()
    {
        using var context = CriarContexto();
        var service = new AuthService(context);
        await service.RegisterAsync(new RegisterRequest
        {
            Nome = "Ana Silva",
            Email = "ana@habitus.test",
            Senha = "Senha123"
        });

        var result = await service.LoginAsync(new LoginRequest
        {
            Email = "ana@habitus.test",
            Senha = "Senha123"
        });

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal("Ana Silva", result.Data.Nome);
        Assert.Equal("ana@habitus.test", result.Data.Email);
    }

    [Fact]
    public async Task NaoDeveRealizarLoginComSenhaIncorreta()
    {
        using var context = CriarContexto();
        var service = new AuthService(context);
        await service.RegisterAsync(new RegisterRequest
        {
            Nome = "Ana Silva",
            Email = "ana@habitus.test",
            Senha = "Senha123"
        });

        var result = await service.LoginAsync(new LoginRequest
        {
            Email = "ana@habitus.test",
            Senha = "SenhaIncorreta"
        });

        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.Equal(AuthErrorType.Unauthorized, result.ErrorType);
    }

    private static AppDbContext CriarContexto()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
