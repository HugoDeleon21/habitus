using Habitus.Api.Data;
using Habitus.Api.DTOs;
using Habitus.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Habitus.Api.Services
{
    public class HabitoService : IHabitoService
    {
        private readonly AppDbContext _context;

        public HabitoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<HabitoServiceResult<HabitoResponse>> CriarAsync(HabitoCreateRequest request)
        {
            var validationMessage = ValidarCriacao(request);
            if (validationMessage is not null)
            {
                return HabitoServiceResult<HabitoResponse>.Fail(validationMessage, HabitoErrorType.Validation);
            }

            var usuarioExiste = await _context.Usuarios.AnyAsync(usuario => usuario.Id == request.UsuarioId);
            if (!usuarioExiste)
            {
                return HabitoServiceResult<HabitoResponse>.Fail("UsuarioId informado nao existe.", HabitoErrorType.Validation);
            }

            var habito = new Habito
            {
                Nome = request.Nome.Trim(),
                Descricao = request.Descricao.Trim(),
                Data = request.Data,
                UsuarioId = request.UsuarioId,
                Concluido = false
            };

            _context.Habitos.Add(habito);
            await _context.SaveChangesAsync();

            return HabitoServiceResult<HabitoResponse>.Ok(MapearParaResponse(habito), "Habito criado com sucesso.");
        }

        public async Task<List<HabitoResponse>> ListarAsync()
        {
            return await _context.Habitos
                .AsNoTracking()
                .OrderBy(habito => habito.Data)
                .ThenBy(habito => habito.Id)
                .Select(habito => MapearParaResponse(habito))
                .ToListAsync();
        }

        public async Task<HabitoServiceResult<HabitoResponse>> BuscarPorIdAsync(int id)
        {
            var habito = await _context.Habitos
                .AsNoTracking()
                .FirstOrDefaultAsync(habito => habito.Id == id);

            if (habito is null)
            {
                return HabitoServiceResult<HabitoResponse>.Fail("Habito nao encontrado.", HabitoErrorType.NotFound);
            }

            return HabitoServiceResult<HabitoResponse>.Ok(MapearParaResponse(habito));
        }

        public async Task<List<HabitoResponse>> ListarPorDataAsync(DateOnly data)
        {
            return await _context.Habitos
                .AsNoTracking()
                .Where(habito => habito.Data == data)
                .OrderBy(habito => habito.Id)
                .Select(habito => MapearParaResponse(habito))
                .ToListAsync();
        }

        public async Task<HabitoServiceResult<HabitoResponse>> AtualizarAsync(int id, HabitoUpdateRequest request)
        {
            var validationMessage = ValidarAtualizacao(request);
            if (validationMessage is not null)
            {
                return HabitoServiceResult<HabitoResponse>.Fail(validationMessage, HabitoErrorType.Validation);
            }

            var habito = await _context.Habitos.FirstOrDefaultAsync(habito => habito.Id == id);
            if (habito is null)
            {
                return HabitoServiceResult<HabitoResponse>.Fail("Habito nao encontrado.", HabitoErrorType.NotFound);
            }

            habito.Nome = request.Nome.Trim();
            habito.Descricao = request.Descricao.Trim();
            habito.Data = request.Data;

            await _context.SaveChangesAsync();

            return HabitoServiceResult<HabitoResponse>.Ok(MapearParaResponse(habito), "Habito atualizado com sucesso.");
        }

        public async Task<HabitoServiceResult<bool>> ExcluirAsync(int id)
        {
            var habito = await _context.Habitos.FirstOrDefaultAsync(habito => habito.Id == id);
            if (habito is null)
            {
                return HabitoServiceResult<bool>.Fail("Habito nao encontrado.", HabitoErrorType.NotFound);
            }

            _context.Habitos.Remove(habito);
            await _context.SaveChangesAsync();

            return HabitoServiceResult<bool>.Ok(true, "Habito excluido com sucesso.");
        }

        public async Task<HabitoServiceResult<HabitoResponse>> AlternarConclusaoAsync(int id)
        {
            var habito = await _context.Habitos.FirstOrDefaultAsync(habito => habito.Id == id);
            if (habito is null)
            {
                return HabitoServiceResult<HabitoResponse>.Fail("Habito nao encontrado.", HabitoErrorType.NotFound);
            }

            habito.Concluido = !habito.Concluido;
            await _context.SaveChangesAsync();

            return HabitoServiceResult<HabitoResponse>.Ok(MapearParaResponse(habito), "Status do habito atualizado com sucesso.");
        }

        public async Task<MetricaDiaResponse> ObterMetricasDoDiaAsync(DateOnly data)
        {
            var totalHabitos = await _context.Habitos.CountAsync(habito => habito.Data == data);
            var habitosConcluidos = await _context.Habitos.CountAsync(habito => habito.Data == data && habito.Concluido);
            var taxaConclusao = totalHabitos == 0
                ? 0
                : Math.Round(habitosConcluidos * 100.0 / totalHabitos, 2);

            return new MetricaDiaResponse
            {
                TotalHabitos = totalHabitos,
                HabitosConcluidos = habitosConcluidos,
                TaxaConclusao = taxaConclusao
            };
        }

        private static string? ValidarCriacao(HabitoCreateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Nome) ||
                string.IsNullOrWhiteSpace(request.Descricao) ||
                request.Data == default ||
                request.UsuarioId <= 0)
            {
                return "Nome, descricao, data e UsuarioId sao obrigatorios.";
            }

            return null;
        }

        private static string? ValidarAtualizacao(HabitoUpdateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Nome) ||
                string.IsNullOrWhiteSpace(request.Descricao) ||
                request.Data == default)
            {
                return "Nome, descricao e data sao obrigatorios.";
            }

            return null;
        }

        private static HabitoResponse MapearParaResponse(Habito habito)
        {
            return new HabitoResponse
            {
                Id = habito.Id,
                Nome = habito.Nome,
                Descricao = habito.Descricao,
                Data = habito.Data,
                Concluido = habito.Concluido,
                DataCriacao = habito.DataCriacao,
                UsuarioId = habito.UsuarioId
            };
        }
    }
}
