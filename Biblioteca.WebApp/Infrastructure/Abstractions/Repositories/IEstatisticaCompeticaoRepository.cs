using IFL.WebApp.Model;

namespace IFL.WebApp.Infrastructure.Abstractions.Repositories
{
    public interface IEstatisticaCompeticaoRepository : IRepository<EstatisticaCompeticao>
    {
        Task<EstatisticaCompeticao?> GetForUpdateAsync(int? id);
    }
}
