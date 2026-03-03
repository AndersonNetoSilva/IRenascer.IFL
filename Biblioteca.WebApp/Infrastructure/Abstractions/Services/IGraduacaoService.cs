using IFL.WebApp.Model;
using IFL.WebApp.Model;

namespace IFL.WebApp.Infrastructure.Abstractions.Services
{
    public interface IGraduacaoService
    {
        Task UpdateAsync(Graduacao graduacao, IEnumerable<GraduacaoAtletaVM> graduacaoAtletaVMs);

        Task AddAsync(Graduacao graduacao, IEnumerable<GraduacaoAtletaVM> graduacaoAtletaVMs);
        bool ExisteGraduacaoNoPeriodo(GraduacaoVM graduacao);
    }
}
