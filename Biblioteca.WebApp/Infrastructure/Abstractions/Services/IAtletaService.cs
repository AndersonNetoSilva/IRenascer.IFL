using IFL.WebApp.Model;
using IFL.WebApp.Model;

namespace IFL.WebApp.Infrastructure.Abstractions.Services
{
    public interface IAtletaService
    {
        Task UpdateAsync(Atleta atleta, IEnumerable<AtletaGradeVM> grade, ArquivoVM? arquivoImagem);

        Task AddAsync(Atleta atleta, IEnumerable<AtletaGradeVM> grade, ArquivoVM? arquivoImagem);
    }
}
