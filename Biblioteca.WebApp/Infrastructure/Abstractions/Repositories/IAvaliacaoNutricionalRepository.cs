using IFL.WebApp.Model;

namespace IFL.WebApp.Infrastructure.Abstractions.Repositories
{
    public interface IAvaliacaoNutricionalRepository : IRepository<AvaliacaoNutricional>
    {
        Task<AvaliacaoNutricional?> GetForUpdateAsync(int? id);
    }
}
