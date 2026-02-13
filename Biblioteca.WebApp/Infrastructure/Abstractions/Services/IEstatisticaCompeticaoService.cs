using IFL.WebApp.Model;
using IFL.WebApp.Model;

namespace IFL.WebApp.Infrastructure.Abstractions.Services
{
    public interface IEstatisticaCompeticaoService
    {
        Task UpdateAsync(EstatisticaCompeticao estatistica,
                            IEnumerable<EstatisticaCompeticaoDetalheVM> detalhes
                        );

        Task AddAsync(EstatisticaCompeticao estatistica,
                        IEnumerable<EstatisticaCompeticaoDetalheVM> detalhes
                        );
    }
}
