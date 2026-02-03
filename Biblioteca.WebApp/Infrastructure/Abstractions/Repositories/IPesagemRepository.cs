using IFL.WebApp.Model;

namespace IFL.WebApp.Infrastructure.Abstractions.Repositories
{
    public interface IPesagemRepository : IRepository<Pesagem>
    {
        Task<Pesagem?> GetForUpdateAsync(int? id);
    }
}
