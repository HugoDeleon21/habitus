using Habitus.Api.Data;
using Habitus.Api.DTOs;
using Habitus.Api.Models;
using Habitus.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Habitus.Tests;

public class HabitoServiceTests
{
    [Fact]
    public async Task DeveCriarHabitoComDadosValidos()
    {
        using var context = CriarContexto();
        var usuario = await CriarUsuarioAsync(context);
        var service = new HabitoService(context);
        var data = new DateOnly(2026, 5, 19);

        var result = await service.CriarAsync(new HabitoCreateRequest
        {
            Nome = "Estudar",
            Descricao = "Estudar algoritmos",
            Data = data,
            UsuarioId = usuario.Id
        });

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal("Estudar", result.Data.Nome);
        Assert.Equal("Estudar algoritmos", result.Data.Descricao);
        Assert.Equal(data, result.Data.Data);
        Assert.Equal(usuario.Id, result.Data.UsuarioId);
        Assert.False(result.Data.Concluido);
        Assert.Single(context.Habitos);
    }

    [Fact]
    public async Task DeveMarcarHabitoComoConcluido()
    {
        using var context = CriarContexto();
        var usuario = await CriarUsuarioAsync(context);
        var service = new HabitoService(context);
        var habitoCriado = await service.CriarAsync(new HabitoCreateRequest
        {
            Nome = "Ler",
            Descricao = "Ler artigo academico",
            Data = new DateOnly(2026, 5, 19),
            UsuarioId = usuario.Id
        });

        var result = await service.AlternarConclusaoAsync(habitoCriado.Data!.Id);

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.True(result.Data.Concluido);

        var habitoPersistido = await context.Habitos.SingleAsync();
        Assert.True(habitoPersistido.Concluido);
    }

    private static AppDbContext CriarContexto()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private static async Task<Usuario> CriarUsuarioAsync(AppDbContext context)
    {
        var usuario = new Usuario
        {
            Nome = "Ana Silva",
            Email = "ana@habitus.test",
            SenhaHash = "hash"
        };

        context.Usuarios.Add(usuario);
        await context.SaveChangesAsync();

        return usuario;
    }
}
