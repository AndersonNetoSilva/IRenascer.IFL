using IFL.WebApp.Data;
using IFL.WebApp.Infrastructure.Abstractions.Repositories;
using IFL.WebApp.Model;

namespace IFL.WebApp.Infrastructure.Repositories
{
    public class PrecoDeVendaRepository : DbContextRepository<PrecoDeVenda, ApplicationDbContext>, IPrecoDeVendaRepository
    {
        public PrecoDeVendaRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
