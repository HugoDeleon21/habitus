using System.Globalization;
using Habitus.Api.DTOs;
using Habitus.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Habitus.Api.Controllers
{
    [ApiController]
    [Route("api/habitos")]
    public class HabitosController : ControllerBase
    {
        private readonly IHabitoService _habitoService;

        public HabitosController(IHabitoService habitoService)
        {
            _habitoService = habitoService;
        }

        [HttpPost]
        public async Task<IActionResult> Criar(HabitoCreateRequest request)
        {
            var result = await _habitoService.CriarAsync(request);

            if (!result.Success)
            {
                return MapearErro(result.Message, result.ErrorType);
            }

            return CreatedAtAction(nameof(BuscarPorId), new { id = result.Data!.Id }, result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var habitos = await _habitoService.ListarAsync();
            return Ok(habitos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var result = await _habitoService.BuscarPorIdAsync(id);

            if (!result.Success)
            {
                return MapearErro(result.Message, result.ErrorType);
            }

            return Ok(result.Data);
        }

        [HttpGet("data/{data}")]
        public async Task<IActionResult> ListarPorData(string data)
        {
            if (!TryParseData(data, out var dataConvertida))
            {
                return BadRequest(new { mensagem = "Data invalida. Use o formato yyyy-MM-dd." });
            }

            var habitos = await _habitoService.ListarPorDataAsync(dataConvertida);
            return Ok(habitos);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Atualizar(int id, HabitoUpdateRequest request)
        {
            var result = await _habitoService.AtualizarAsync(id, request);

            if (!result.Success)
            {
                return MapearErro(result.Message, result.ErrorType);
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var result = await _habitoService.ExcluirAsync(id);

            if (!result.Success)
            {
                return MapearErro(result.Message, result.ErrorType);
            }

            return NoContent();
        }

        [HttpPatch("{id:int}/concluir")]
        public async Task<IActionResult> AlternarConclusao(int id)
        {
            var result = await _habitoService.AlternarConclusaoAsync(id);

            if (!result.Success)
            {
                return MapearErro(result.Message, result.ErrorType);
            }

            return Ok(result.Data);
        }

        [HttpGet("metricas/dia/{data}")]
        public async Task<IActionResult> ObterMetricasDoDia(string data)
        {
            if (!TryParseData(data, out var dataConvertida))
            {
                return BadRequest(new { mensagem = "Data invalida. Use o formato yyyy-MM-dd." });
            }

            var metricas = await _habitoService.ObterMetricasDoDiaAsync(dataConvertida);
            return Ok(metricas);
        }

        private static bool TryParseData(string data, out DateOnly dataConvertida)
        {
            return DateOnly.TryParseExact(data, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataConvertida);
        }

        private IActionResult MapearErro(string message, HabitoErrorType? errorType)
        {
            if (errorType == HabitoErrorType.NotFound)
            {
                return NotFound(new { mensagem = message });
            }

            return BadRequest(new { mensagem = message });
        }
    }
}
