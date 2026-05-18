using Habitus.Api.DTOs;

namespace Habitus.Api.Services
{
    public interface IHabitoService
    {
        Task<HabitoServiceResult<HabitoResponse>> CriarAsync(HabitoCreateRequest request);

        Task<List<HabitoResponse>> ListarAsync();

        Task<HabitoServiceResult<HabitoResponse>> BuscarPorIdAsync(int id);

        Task<List<HabitoResponse>> ListarPorDataAsync(DateOnly data);

        Task<HabitoServiceResult<HabitoResponse>> AtualizarAsync(int id, HabitoUpdateRequest request);

        Task<HabitoServiceResult<bool>> ExcluirAsync(int id);

        Task<HabitoServiceResult<HabitoResponse>> AlternarConclusaoAsync(int id);

        Task<MetricaDiaResponse> ObterMetricasDoDiaAsync(DateOnly data);
    }
}
