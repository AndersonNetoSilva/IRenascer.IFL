using IFL.WebApp.Model;
using IFL.WebApp.Model;

namespace IFL.WebApp.Infrastructure.Abstractions.Services
{
    public interface IAvaliacaoNutricionalService
    {
        Task UpdateAsync(AvaliacaoNutricional avaliacaoNutricional,
                            ArquivoVM? arquivoImagem);

        Task AddAsync(AvaliacaoNutricional avaliacaoNutricional, 
                        ArquivoVM? arquivoImagem);
    }
}
