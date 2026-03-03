using IFL.WebApp.Model;

namespace IFL.WebApp.Infrastructure.Abstractions.Repositories
{
    public interface IGraduacaoRepository : IRepository<Graduacao>
    {
        Task<Graduacao?> GetForUpdateAsync(int? id);
        
    }
}
