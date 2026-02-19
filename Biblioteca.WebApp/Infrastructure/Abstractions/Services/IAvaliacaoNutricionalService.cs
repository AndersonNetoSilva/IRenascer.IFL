using IFL.WebApp.Model;
using IFL.WebApp.Model;

namespace IFL.WebApp.Infrastructure.Abstractions.Services
{
    public interface IAvaliacaoNutricionalService
    {
        Task UpdateAsync(AvaliacaoNutricional avaliacaoNutricional,
                            ArquivoVM? arquivoImagem,
                            ArquivoVM? arquivoImagemCostas,
                            IEnumerable<AvaliacaoNutricionalAnexoVM> anexos
                        );

        Task AddAsync(AvaliacaoNutricional avaliacaoNutricional, 
                        ArquivoVM? arquivoImagem,
                        ArquivoVM? arquivoImagemCostas,
                        IEnumerable<AvaliacaoNutricionalAnexoVM> anexos
                        );
    }
}
